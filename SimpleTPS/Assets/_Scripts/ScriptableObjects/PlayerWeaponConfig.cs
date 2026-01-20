using _Scripts.Player.Definition;
using UnityEngine;

namespace _Datas
{
    
    [CreateAssetMenu(fileName = "PlayerWeaponConfig", menuName = "ScriptableObject/PlayerWeaponConfig")]
    public class PlayerWeaponConfig : ScriptableObject
    {
        [Header("Identity")]
        public string WeaponSid;
        public string WeaponDisplayName;
        public GameObject WeaponPrefab;

        [Header("Fire")]
        public FireMode FireMode = FireMode.Auto;
        [Min(1f)] public float Damage = 10f;

        // 1초당 발사 간격(권장). RPM을 쓰고 싶으면 별도 필드로 두고 변환 캐싱
        [Min(0.01f)] public float FireInterval = 0.1f;

        [Header("Ammo")]
        [Min(1)] public int MagazineSize = 30;
        [Min(0f)] public float ReloadTime = 1.8f;

        [Header("Aim")]
        [Range(20f, 90f)] public float ADSFov = 55f;
        [Range(0.1f, 1f)] public float ADSSensitivityMultiplier = 0.6f;

        [Header("Spread")]
        public SpreadDefinition HipSpread;
        public SpreadDefinition ADSSpread;

        [Header("Recoil")]
        public RecoilDefinition HipRecoil;
        public RecoilDefinition ADSRecoil;
    }
}