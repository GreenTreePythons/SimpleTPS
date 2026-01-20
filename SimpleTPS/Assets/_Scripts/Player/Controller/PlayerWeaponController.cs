using _Datas;
using _Scripts.Player.Input;
using _Scripts.Player.Weapon;
using UnityEngine;

namespace _Scripts.Player.Controller
{
    public sealed class PlayerWeaponController : MonoBehaviour
    {
        [Header("Default Weapon")]
        [SerializeField] private PlayerWeaponConfig m_DefaultWeaponConfig;

        [Header("Attach")]
        [SerializeField] private Transform m_WeaponAttachTransform;

        [Header("References")]
        [SerializeField] private Transform m_AimCameraTransform;

        private WeaponController m_CurrentWeaponController;
        private PlayerWeaponConfig m_CurrentWeaponConfig;
        private GameObject m_CurrentWeaponInstance;

        public WeaponController CurrentWeaponController => m_CurrentWeaponController;
        public PlayerWeaponConfig CurrentWeaponConfig => m_CurrentWeaponConfig;

        private void Awake()
        {
            if (m_DefaultWeaponConfig != null)
                Equip(m_DefaultWeaponConfig);
        }

        /// <summary>
        /// Aim 카메라 Transform을 외부(PlayerAimController 등)에서 주입하고 싶을 때 사용.
        /// </summary>
        public void SetAimCameraTransform(Transform cameraTransform)
        {
            m_AimCameraTransform = cameraTransform;

            if (m_CurrentWeaponController != null)
                m_CurrentWeaponController.SetAimCameraTransform(m_AimCameraTransform);
        }

        public void Equip(PlayerWeaponConfig weaponConfig)
        {
            if (weaponConfig == null)
            {
                ClearWeaponInstance();
                m_CurrentWeaponConfig = null;
                m_CurrentWeaponController = null;
                return;
            }

            // 동일 무기 재장착이면 무시(원하면 리필/리셋 옵션을 추가)
            if (m_CurrentWeaponConfig == weaponConfig && m_CurrentWeaponController != null)
                return;

            ClearWeaponInstance();

            m_CurrentWeaponConfig = weaponConfig;

            if (weaponConfig.WeaponPrefab == null)
            {
                Debug.LogError($"WeaponConfig '{weaponConfig.name}' has no WeaponPrefab assigned.");
                m_CurrentWeaponConfig = null;
                return;
            }

            Transform parent = (m_WeaponAttachTransform != null) ? m_WeaponAttachTransform : transform;

            m_CurrentWeaponInstance = Instantiate(weaponConfig.WeaponPrefab, parent);
            m_CurrentWeaponInstance.transform.localPosition = Vector3.zero;
            m_CurrentWeaponInstance.transform.localRotation = Quaternion.identity;
            m_CurrentWeaponInstance.transform.localScale = Vector3.one;

            m_CurrentWeaponController = m_CurrentWeaponInstance.GetComponent<WeaponController>();
            if (m_CurrentWeaponController == null)
            {
                Debug.LogError($"WeaponPrefab '{weaponConfig.WeaponPrefab.name}' has no WeaponController component.");
                ClearWeaponInstance();
                m_CurrentWeaponConfig = null;
                return;
            }

            if (m_AimCameraTransform != null)
                m_CurrentWeaponController.SetAimCameraTransform(m_AimCameraTransform);
        }

        public void Tick(PlayerInputSnapshot inputSnapshot, float dt)
        {
            if (m_CurrentWeaponController == null) return;

            m_CurrentWeaponController.Tick(dt, inputSnapshot.IsShootPressed, inputSnapshot.IsReloadPressed, inputSnapshot.IsADSMode);
        }

        public int GetCurrentMaxAmmo() => m_CurrentWeaponConfig.MagazineSize;

        public int GetCurrentAmmo() => m_CurrentWeaponController.AmmoInMag;

        private void ClearWeaponInstance()
        {
            if (m_CurrentWeaponInstance != null)
            {
                Destroy(m_CurrentWeaponInstance);
                m_CurrentWeaponInstance = null;
            }

            m_CurrentWeaponController = null;
        }
    }
}
