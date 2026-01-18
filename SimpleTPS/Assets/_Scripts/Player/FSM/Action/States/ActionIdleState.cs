using _Scripts.Player.Controller;
using _Scripts.Player.Input;

namespace _Scripts.Player.FSM.Action
{
    public class ActionIdleState : ActionBaseState
    {
        public override PlayerStateType StateType => PlayerStateType.AimingIdle;
        
        public ActionIdleState(PlayerActionFSM fsm, PlayerAnimationController anim) : base(fsm, anim) { }

        public override void Enter(in PlayerInputSnapshot input)
        {
            Anim.SetLayerWeight(1, 0f);
        }
    }
}