using _Scripts.Player.Input;

namespace _Scripts.Player.FSM.Locomotion
{
    public class LocomotionBaseState
    {
        protected readonly PlayerLocomotionFSM Fsm;

        protected LocomotionBaseState(PlayerLocomotionFSM fsm)
        {
            Fsm = fsm;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }

        public virtual void Tick(in PlayerInputSnapshot input, float dt)
        {
            
        }
    }
}