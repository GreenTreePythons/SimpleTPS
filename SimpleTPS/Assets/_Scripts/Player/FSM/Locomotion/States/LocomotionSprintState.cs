using _Scripts.Player.Controller;
using _Scripts.Player.Input;
using UnityEngine;

namespace _Scripts.Player.FSM.Locomotion
{
    public sealed class LocomotionSprintState : LocomotionBaseState
    {
        public LocomotionSprintState(PlayerLocomotionFSM fsm) : base(fsm) { }

        public override void Enter()
        {
            
        }

        public override void Tick(in PlayerInputSnapshot input, float dt)
        {
            base.Tick(input, dt);
            
            Vector2 move = new Vector2(input.Move.x, input.Move.y * 2f);
            float mag = Mathf.Clamp01(move.magnitude);

            float speed01 = mag;
            float damp = 0.08f;

            Fsm.Animation.SetLocomotion(move, speed01, damp, dt);

            if (move.sqrMagnitude <= 0.0001f)
            {
                Fsm.ChangeState(Fsm.Idle);
                return;
            }

            if (!input.IsSprintPressed)
            {
                Fsm.ChangeState(Fsm.Walk);
            }
            
            Fsm.Move(input, dt, Fsm.GetWalkSpeed());
        }
    }
}