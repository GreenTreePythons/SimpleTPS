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
            Fsm.Animation.PlayAnimation(PlayerStateType.Sprint);
        }

        public override void Tick(in PlayerInputSnapshot input, float dt)
        {
            base.Tick(input, dt);
        }
    }
}