using System;
using System.Collections.Generic;
using _Scripts.Player.Definition;
using UnityEngine;

namespace _Datas
{
    [CreateAssetMenu(fileName = "PlayerAnimationsConfig", menuName = "ScriptableObject/PlayerAnimationsConfig")]
    public class PlayerAnimationsConfig : ScriptableObject
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

            Debug.LogError($"[PlayerAnimation] Animation not found: {state}");
            return null;
        }
    }

}