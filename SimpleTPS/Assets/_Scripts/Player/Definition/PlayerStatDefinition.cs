using System;
using UnityEngine.Serialization;

namespace _Scripts.Player.Definition
{
    public enum PlayerStatType : byte
    {
        MaxHp = 0,
        WalkSpeed = 1,
        SprintSpeed = 2,
    }

    public enum StatModifierOperator : byte
    {
        Add = 0,
        Multiply = 1,
    }
    
    [Serializable]
    public struct PlayerStats
    {
        public int MaxHp;
        public float WalkSpeed;
        public float SprintSpeed;

        public PlayerStats(int maxHp, float walkSpeed, float sprintSpeed)
        {
            MaxHp = maxHp;
            WalkSpeed = walkSpeed;
            SprintSpeed = sprintSpeed;
        }
    }

    [Serializable]
    public struct StatModifier
    {
        public PlayerStatType StatType;
        [FormerlySerializedAs("Op")] public StatModifierOperator Operator;
        public float Value;

        public int SourceId;

        public StatModifier(PlayerStatType statType, StatModifierOperator op, float value, int sourceId = 0)
        {
            StatType = statType;
            Operator = op;
            Value = value;
            SourceId = sourceId;
        }
    }
}