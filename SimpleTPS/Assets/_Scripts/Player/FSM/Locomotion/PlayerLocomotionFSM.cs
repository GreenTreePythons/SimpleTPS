using _Scripts.Player.Controller;
using _Scripts.Player.Input;
using UnityEngine;

namespace _Scripts.Player.FSM.Locomotion
{
    public class PlayerLocomotionFSM
    {
        public readonly LocomotionIdleState Idle;
        public readonly LocomotionWalkState Walk;
        public readonly LocomotionSprintState Sprint;
        
        private LocomotionBaseState m_Current;
        private readonly CharacterController m_CharacterController;
        private readonly PlayerAimController m_AimController;
        private readonly PlayerAnimationController m_AnimationController;
        private readonly PlayerStatController m_StatController;
        private readonly Transform m_CameraTransform;

        public PlayerLocomotionFSM(
            CharacterController characterController,
            PlayerAimController aimController,
            PlayerAnimationController animationController,
            PlayerStatController statController)
        {
            m_CharacterController = characterController;
            m_AimController = aimController;
            m_AnimationController = animationController;
            m_StatController = statController;

            Idle = new LocomotionIdleState(this);
            Walk = new LocomotionWalkState(this);
            Sprint = new LocomotionSprintState(this);

            m_Current = Idle;
            m_Current.Enter();
        }

        public void Tick(in PlayerInputSnapshot input, float dt)
        {
            m_Current.Tick(input, dt);
        }

        public void ChangeState(LocomotionBaseState next)
        {
            if (next == m_Current) return;
            
            m_Current.Exit();
            m_Current = next;
            m_Current.Enter();
        }

        public void SetLocomotion(Vector2 move, float speed, float damp, float dt)
        {
            m_AnimationController.SetLocomotion(move, speed, damp, dt);
        }
        
        public void Move(in PlayerInputSnapshot input, float dt, float speed)
        {
            Vector2 move = input.Move;
            if (move.sqrMagnitude <= 0.0001f) return;

            // -1~1 입력 정규화(대각선 속도 보정)
            if (move.sqrMagnitude > 1f) move.Normalize();

            var camTransform = m_AimController.GetCameraTransform();
            Vector3 forward = camTransform.forward;
            Vector3 right   = camTransform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 worldDir = (right * move.x + forward * move.y);
            if (worldDir.sqrMagnitude > 1f) worldDir.Normalize();

            Vector3 delta = worldDir * speed * dt;

            m_CharacterController.Move(delta);
        }

        public float GetWalkSpeed() => m_StatController.CurrentStats.WalkSpeed;
        public float GetSprintSpeed() => m_StatController.CurrentStats.SprintSpeed;
    }
}
