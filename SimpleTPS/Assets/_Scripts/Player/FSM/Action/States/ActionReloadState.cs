using _Scripts.Player.Controller;
using _Scripts.Player.Definition;
using _Scripts.Player.Input;

namespace _Scripts.Player.FSM.Action
{
    public sealed class ActionReloadState : ActionBaseState
    {
        public override PlayerStateType StateType => PlayerStateType.Reload;
        
        private float m_Remaining;
        public override bool IsComplete => m_Remaining <= 0f;
        
        public ActionReloadState(PlayerActionFSM fsm, PlayerAnimationController anim) : base(fsm, anim) { }

        public override void Enter(in PlayerInputSnapshot input)
        {
            m_Remaining = Anim.GetAnimationPlayTime(StateType);

            Anim.PlayAnimation(StateType, Fsm.UpperLayerIndex, 1f);
        }

        public override void Tick(in PlayerInputSnapshot input, float dt)
        {
            m_Remaining -= dt;
            if(input.IsShootPressed) IsComplete = true;
            
            if (m_Remaining <= 0f)
            {
                IsComplete = true;
            }
        }
    }
}