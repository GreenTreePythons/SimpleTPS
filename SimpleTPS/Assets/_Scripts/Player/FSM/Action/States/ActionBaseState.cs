using _Scripts.Player.Controller;
using _Scripts.Player.Definition;
using _Scripts.Player.Input;

namespace _Scripts.Player.FSM.Action
{
    public abstract class ActionBaseState
    {
        protected readonly PlayerActionFSM Fsm;
        protected readonly PlayerAnimationController Anim;

        protected ActionBaseState(PlayerActionFSM fsm, PlayerAnimationController anim)
        {
            Fsm = fsm;
            Anim = anim;
        }
        
        public abstract PlayerStateType StateType { get; }
        public virtual bool IsComplete { get; protected set; }

        public virtual void Enter(in PlayerInputSnapshot input) { }
        public virtual void Exit() { }
        public virtual void Tick(in PlayerInputSnapshot input, float dt) { }

    }
}