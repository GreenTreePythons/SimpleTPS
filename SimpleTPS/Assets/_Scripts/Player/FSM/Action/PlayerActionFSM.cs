using _Scripts.Player.Controller;
using _Scripts.Player.Input;
using UnityEngine;

namespace _Scripts.Player.FSM.Action
{
    public sealed class PlayerActionFSM
    {
        public int UpperLayerIndex { get; }

        private readonly PlayerAnimationController m_Anim;
        
        private readonly ActionIdleState m_Idle;
        private readonly ActionShootState m_Shoot;
        private readonly ActionReloadState m_Reload;

        private ActionBaseState m_Current;

        public PlayerActionFSM(PlayerAnimationController anim, int upperLayerIndex = 1)
        {
            m_Anim = anim;
            UpperLayerIndex = upperLayerIndex;
            
            m_Idle = new ActionIdleState(this, anim);
            m_Shoot = new ActionShootState(this, anim);
            m_Reload = new ActionReloadState(this, anim);
            
            m_Current = m_Idle;
        }

        public void Tick(in PlayerInputSnapshot input, float dt)
        {
            // 1) 현재 상태 Tick
            m_Current.Tick(input, dt);

            // 2) 다음 상태 결정
            var next = ResolveNextState(m_Current, input);

            // 3) 전환
            if (!ReferenceEquals(m_Current, next))
                ChangeState(next, input);
        }
        
        private ActionBaseState ResolveNextState(ActionBaseState cur, in PlayerInputSnapshot input)
        {
            // Reload만 특수 규칙 (완주형 + Shoot 인터럽트)
            if (cur == m_Reload)
            {
                if (input.IsShootPressed) return m_Shoot;   // interrupt
                if (!cur.IsComplete) return cur;          // keep
                return ResolveByPriority(input);          // done -> follow input
            }

            // 나머지는 항상 입력 우선순위를 따름
            return ResolveByPriority(input);
        }

        private ActionBaseState ResolveByPriority(in PlayerInputSnapshot input)
        {
            if (input.IsReloadPressed) return m_Reload;
            if (input.IsShootPressed)  return m_Shoot;
            return m_Idle;
        }

        private void ChangeState(ActionBaseState next, in PlayerInputSnapshot input)
        {
            m_Current.Exit();
            m_Current = next;
            m_Current.Enter(input);

            // “Idle = Upper OFF” 정책을 강제하고 싶으면 여기서 일괄 처리도 가능
            // if (ReferenceEquals(current, idle)) Anim.SetLayerWeight(UpperLayerIndex, 0f);
        }
    }
}