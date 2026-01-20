using UnityEngine;
using System;

namespace _Scripts.Player.Definition
{
    public enum FireMode
    {
        Semi,
        Auto,
        Burst
    }

    [Serializable]
    public struct SpreadDefinition
    {
        [Min(0f)] public float BaseAngleDeg;          // 기본 퍼짐 각(원뿔)
        [Min(0f)] public float MaxAngleDeg;           // 최대 퍼짐
        [Min(0f)] public float IncreasePerShotDeg;    // 발사 시 증가량
        [Min(0f)] public float RecoveryPerSecDeg;     // 초당 회복량
    }

    [Serializable]
    public struct RecoilDefinition
    {
        // "한 발당" 기본 반동(패턴/랜덤은 나중에 확장)
        public Vector2 KickDeg;           // x=Yaw(좌우), y=Pitch(상하)
        public Vector2 RandomRangeDeg;    // 랜덤 범위(+/-)
        [Min(0f)] public float ReturnPerSec; // 초당 원복(누적 복구)
    }
}