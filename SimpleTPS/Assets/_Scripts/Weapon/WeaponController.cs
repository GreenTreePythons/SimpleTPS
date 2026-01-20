using System.Collections;
using System.Collections.Generic;
using _Datas;
using UnityEngine;

namespace _Scripts.Player.Weapon
{
    public sealed class WeaponController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private PlayerWeaponConfig m_WeaponConfig;

        [Header("References")]
        [SerializeField] private Transform m_MuzzleTransform;
        [SerializeField] private ParticleSystem m_MuzzleVfx;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip m_FireClip;
        [SerializeField] private AudioClip m_ReloadClip;
        [SerializeField] private AudioClip m_EmptyClip;

        [Header("Raycast")]
        [SerializeField] private LayerMask m_HitMask = ~0;
        [SerializeField] private float m_Range = 200f;

        [Header("Tracer (TrailRenderer)")]
        [SerializeField] private TrailRenderer m_TracerPrefab;
        [SerializeField] private int m_TracerPoolSize = 16;
        [SerializeField] private float m_TracerSpeed = 600f;

        private Transform m_AimCameraTransform;

        private readonly WeaponRuntimeState m_Runtime = new();
        private readonly Queue<TrailRenderer> m_TracerPool = new();
        
        private float m_TracerTimeRemaining;
        private AudioSource m_AudioSource;

        public PlayerWeaponConfig WeaponConfig => m_WeaponConfig;
        public int AmmoInMag => m_Runtime.AmmoInMag;
        public int ReserveAmmo => m_Runtime.ReserveAmmo;
        public bool IsReloading => m_Runtime.IsReloading;

        private void Awake()
        {
            if (m_WeaponConfig != null)
            {
                m_Runtime.AmmoInMag = m_WeaponConfig.MagazineSize;
            }

            // 프로토타입 기본값: 예비탄 무한(원하면 Player에서 주입)
            m_Runtime.ReserveAmmo = int.MaxValue;
            
            m_AudioSource = GetComponent<AudioSource>();
            InitializeTracerPool();
        }
        
        private void InitializeTracerPool()
        {
            if (m_TracerPrefab == null) return;

            for (int i = 0; i < m_TracerPoolSize; i++)
            {
                TrailRenderer tracer = Instantiate(m_TracerPrefab);
                tracer.gameObject.SetActive(false);
                tracer.emitting = false;
                m_TracerPool.Enqueue(tracer);
            }
        }

        public void SetAimCameraTransform(Transform cameraTransform)
        {
            m_AimCameraTransform = cameraTransform;
        }

        /// <summary>
        /// Player 쪽에서 매 프레임 호출(입력 값을 정제해서 전달)
        /// </summary>
        public void Tick(float dt, bool shootHeld, bool reloadPressed, bool isAds)
        {
            if (m_WeaponConfig == null)
                return;

            // Fire Cooldown
            if (m_Runtime.FireCooldown > 0f)
                m_Runtime.FireCooldown -= dt;

            // Reload 진행
            if (m_Runtime.IsReloading)
            {
                m_Runtime.ReloadRemaining -= dt;
                if (m_Runtime.ReloadRemaining <= 0f)
                    FinishReload();
            }

            // Reload 요청
            if (reloadPressed)
                TryStartReload();

            // Shoot 처리
            if (shootHeld)
            {
                switch (m_WeaponConfig.FireMode)
                {
                    case Definition.FireMode.Auto:
                        TryShoot(isAds, triggerPressedThisFrame: false);
                        break;

                    case Definition.FireMode.Semi:
                        // Semi는 "이번 프레임에 눌림"만 발사
                        bool pressedThisFrame = !m_Runtime.WasTriggerHeld;
                        TryShoot(isAds, triggerPressedThisFrame: pressedThisFrame);
                        break;

                    case Definition.FireMode.Burst:
                        // 최소 구현 단계에서는 Burst 미지원(후속 작업)
                        // 필요 시 BurstQueue(남은 발 수)로 확장
                        break;
                }
            }

            m_Runtime.WasTriggerHeld = shootHeld;
        }

        private void TryShoot(bool isAds, bool triggerPressedThisFrame)
        {
            if (m_WeaponConfig.FireMode == Definition.FireMode.Semi && !triggerPressedThisFrame)
                return;

            if (m_Runtime.IsReloading || m_Runtime.FireCooldown > 0f)
                return;

            if (m_Runtime.AmmoInMag <= 0)
            {
                PlayOneShot(m_EmptyClip);
                return;
            }

            m_Runtime.AmmoInMag -= 1;
            m_Runtime.FireCooldown = m_WeaponConfig.FireInterval;

            // 2) 연출(총구 기준)
            PlayMuzzleVfx();
            PlayOneShot(m_FireClip);
            
            Vector3 start = m_MuzzleTransform.position;
            Vector3 end = start + m_MuzzleTransform.forward * m_Range;
            PlayTracer(start, end);

            // 3) 데미지 적용(최소 구현: 인터페이스/컴포넌트는 후속)
            // 예: hit.collider.GetComponent<IDamageable>()?.ApplyDamage(m_WeaponConfig.Damage);
        }

        public bool TryStartReload()
        {
            if (m_WeaponConfig == null)
                return false;

            if (m_Runtime.IsReloading)
                return false;

            if (m_Runtime.AmmoInMag >= m_WeaponConfig.MagazineSize)
                return false;

            if (m_Runtime.ReserveAmmo <= 0)
                return false;

            m_Runtime.IsReloading = true;
            m_Runtime.ReloadRemaining = Mathf.Max(0.01f, m_WeaponConfig.ReloadTime);

            PlayOneShot(m_ReloadClip);
            return true;
        }

        private void FinishReload()
        {
            m_Runtime.IsReloading = false;
            m_Runtime.ReloadRemaining = 0f;

            int need = m_WeaponConfig.MagazineSize - m_Runtime.AmmoInMag;
            if (need <= 0)
                return;

            int take = Mathf.Min(need, m_Runtime.ReserveAmmo);
            m_Runtime.AmmoInMag += take;
            m_Runtime.ReserveAmmo -= take;
        }

        private void PlayMuzzleVfx()
        {
            if (m_MuzzleVfx == null) return;

            m_MuzzleVfx.gameObject.SetActive(true);
            m_MuzzleVfx.transform.position = m_MuzzleTransform.position;
            m_MuzzleVfx.Play(true);
        }

        private void PlayTracer(Vector3 start, Vector3 end)
        {
            if (m_TracerPool.Count == 0) return;

            TrailRenderer tracer = m_TracerPool.Dequeue();
            tracer.gameObject.SetActive(true);

            tracer.emitting = false;
            tracer.Clear();
            tracer.transform.position = start;
            
            StartCoroutine(CoPlay());
            IEnumerator CoPlay()
            {
                yield return null;
                tracer.emitting = true;
                yield return _CoMoveTracer(tracer, end);
            }
            IEnumerator _CoMoveTracer(TrailRenderer tracer, Vector3 end)
            {
                Vector3 start = tracer.transform.position;
                float distance = Vector3.Distance(start, end);
                float duration = distance / Mathf.Max(1f, m_TracerSpeed);

                float t = 0f;
                while (t < duration)
                {
                    t += Time.deltaTime;
                    tracer.transform.position = Vector3.Lerp(start, end, t / duration);
                    yield return null;
                }

                tracer.transform.position = end;

                yield return new WaitForSeconds(tracer.time);

                tracer.emitting = false;
                tracer.gameObject.SetActive(false);
                m_TracerPool.Enqueue(tracer);
            }
        }

        private void PlayOneShot(AudioClip clip)
        {
            if (clip == null || m_AudioSource == null)
                return;

            m_AudioSource.PlayOneShot(clip);
        }

        private sealed class WeaponRuntimeState
        {
            public int AmmoInMag;
            public int ReserveAmmo;
            public float FireCooldown;
            public bool IsReloading;
            public float ReloadRemaining;
            public bool WasTriggerHeld;
        }
    }
}
