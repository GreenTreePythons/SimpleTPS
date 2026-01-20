using _Scripts.Player.Controller;
using _Scripts.Player.Definition;
using _Scripts.Player.Input;

namespace _Scripts.Player.FSM.Action
{
    public sealed class ActionShootState : ActionBaseState
    {
        public override PlayerStateType StateType => PlayerStateType.Shoot;
        
        public ActionShootState(PlayerActionFSM fsm, PlayerAnimationController anim) : base(fsm, anim) { }

        public override void Enter(in PlayerInputSnapshot input)
        {
            Anim.PlayAnimation(StateType, Fsm.UpperLayerIndex, 1f);
        }

        public override void Tick(in PlayerInputSnapshot input, float dt)
        {
            // "사격을 누르는 동안 계속"이라면 여기서는 애니메이션을 반복 재요청하지 않는다.
            // (Animator가 루프 클립이거나, 트리거/파라미터 기반이면 그쪽에서 처리)
            IsComplete = !input.IsShootPressed;
        }
    }
}