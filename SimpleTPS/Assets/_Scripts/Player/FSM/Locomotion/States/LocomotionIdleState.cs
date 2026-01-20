using _Scripts.Player.Input;
using UnityEngine;

namespace _Scripts.Player.FSM.Locomotion
{
    public sealed class LocomotionIdleState : LocomotionBaseState
    {
        public LocomotionIdleState(PlayerLocomotionFSM fsm) : base(fsm) { }

        public override void Enter()
        {
            
        }

        public override void Tick(in PlayerInputSnapshot input, float dt)
        {
            base.Tick(input, dt);
            
            Fsm.SetLocomotion(Vector2.zero, 0f, 0.1f, dt);
            
            // 이동 입력이 생기면 Walk/Sprint
            if (input.Move.sqrMagnitude > 0.0001f)
            {
                Fsm.ChangeState(input.IsSprintPressed ? Fsm.Sprint : Fsm.Walk);
            }
        }
    }
}