using _Datas;
using _Scripts.Player.Input;
using _Scripts.Player.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Player.Controller
{
    public sealed class PlayerWeaponController : MonoBehaviour
    {
        [Header("Default Weapon")]
        [SerializeField] private PlayerWeaponConfig m_DefaultWeaponConfig;

        [Header("Attach")]
        [SerializeField] private Transform m_WeaponAttachTransform;

        [FormerlySerializedAs("m_AimCameraTransform")]
        [Header("References")]
        [SerializeField] private Transform m_AimCameraTarget;

        private WeaponController m_CurrentWeaponController;
        private PlayerWeaponConfig m_CurrentWeaponConfig;
        private GameObject m_CurrentWeaponObj;

        public WeaponController CurrentWeaponController => m_CurrentWeaponController;
        public PlayerWeaponConfig CurrentWeaponConfig => m_CurrentWeaponConfig;

        private void Awake()
        {
            if (m_DefaultWeaponConfig != null)
                Equip(m_DefaultWeaponConfig);
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

            m_CurrentWeaponObj = Instantiate(weaponConfig.WeaponPrefab, parent);
            m_CurrentWeaponObj.transform.localPosition = Vector3.zero;
            m_CurrentWeaponObj.transform.localRotation = Quaternion.identity;
            m_CurrentWeaponObj.transform.localScale = Vector3.one;

            m_CurrentWeaponController = m_CurrentWeaponObj.GetComponent<WeaponController>();
            if (m_CurrentWeaponController == null)
            {
                Debug.LogError($"WeaponPrefab '{weaponConfig.WeaponPrefab.name}' has no WeaponController component.");
                ClearWeaponInstance();
                m_CurrentWeaponConfig = null;
                return;
            }
        }

        public void Tick(PlayerInputSnapshot inputSnapshot, float dt)
        {
            if (m_CurrentWeaponController == null) return;

            m_CurrentWeaponController.Tick(dt, inputSnapshot.IsShootPressed, inputSnapshot.IsReloadPressed, inputSnapshot.IsADSMode, m_AimCameraTarget.position);
        }

        public int GetCurrentMaxAmmo() => m_CurrentWeaponConfig.MagazineSize;

        public int GetCurrentAmmo() => m_CurrentWeaponController.AmmoInMag;

        private void ClearWeaponInstance()
        {
            if (m_CurrentWeaponObj != null)
            {
                Destroy(m_CurrentWeaponObj);
                m_CurrentWeaponObj = null;
            }

            m_CurrentWeaponController = null;
        }
    }
}
