using System;
using _Scripts.Player.Controller;
using UnityEngine;

namespace _Datas
{
    [CreateAssetMenu(fileName = "PlayerStatsConfig", menuName = "ScriptableObject/PlayerStatsConfig")]
    public class PlayerStatsConfig : ScriptableObject
    {
        [Header("Base Stats")]
        [Min(1)] public int MaxHp = 100;
        [Min(1f)] public float WalkSpeed = 3.5f;
        [Min(1f)] public float SprintSpeed = 5.5f;
    }
}