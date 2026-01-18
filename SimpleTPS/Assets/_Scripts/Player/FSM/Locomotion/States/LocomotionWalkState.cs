using _Scripts.Player.Controller;
using _Scripts.Player.Input;
using UnityEngine;

namespace _Scripts.Player.FSM.Locomotion
{
    public sealed class LocomotionWalkState : LocomotionBaseState
    {
        public LocomotionWalkState(PlayerLocomotionFSM fsm) : base(fsm) { }

        public override void Enter()
        {
            Fsm.Animation.PlayAnimation(PlayerStateType.Walk);
        }

        public override void Tick(in PlayerInputSnapshot input, float dt)
        {
            base.Tick(input, dt);

            Vector2 move = input.Move;
            float mag = Mathf.Clamp01(move.magnitude);

            float speed01 = mag * 0.5f;
            float damp = 0.08f;

            Fsm.Animation.SetLocomotion(move, speed01, damp, dt);

            if (move.sqrMagnitude <= 0.0001f)
            {
                Fsm.ChangeState(Fsm.Idle);
                return;
            }

            if (input.IsSprintPressed)
            {
                Fsm.ChangeState(Fsm.Sprint);
            }
            
            Fsm.Move(input, dt, Fsm.GetWalkSpeed());
        }
    }
}