using System;
using System.Collections.Generic;
using _Scripts.Player.Controller;
using UnityEngine;

namespace _Datas
{
    [CreateAssetMenu(fileName = "PlayerAnimations", menuName = "ScriptableObject/PlayerAnimations")]
    public class PlayerAnimations : ScriptableObject
    {
        [SerializeField] PlayerAnimation[] m_PlayerAnimations;
        
        private Dictionary<PlayerStateType, PlayerAnimation> m_Animations;
        
        private void OnEnable()
        {
            m_Animations = new();
            foreach (var anim in m_PlayerAnimations)
            {
                m_Animations.TryAdd(anim.StateType, anim);
            }
        }

        public PlayerAnimation GetClip(PlayerStateType state)
        {
            if (m_Animations.TryGetValue(state, out var anim)) return anim;

            Debug.LogError($"[PlayerAnimations] Animation not found: {state}");
            return null;
        }
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