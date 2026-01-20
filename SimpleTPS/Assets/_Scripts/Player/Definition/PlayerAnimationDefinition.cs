using System;
using UnityEngine;

namespace _Scripts.Player.Definition
{
    public enum PlayerStateType : byte
    {
        Idle, Walk, Run, Reload, Shoot, AimingIdle
    }
    
    [Serializable]
    public class PlayerAnimation
    {
        [SerializeField] PlayerStateType m_PlayerStateType;
        [SerializeField] string m_StateName;
        [SerializeField] AnimationClip m_Clip;
        [SerializeField] float m_NormalizedTransitionDuration = 0.15f;
        
        public PlayerStateType StateType => m_PlayerStateType;
        public string StateName => m_StateName;
        public AnimationClip Clip => m_Clip;
        public float TransitionDuration => m_NormalizedTransitionDuration;
    }
}