using _Scripts.Player.Controller;
using _Scripts.Player.Input;
using UnityEngine;

namespace _Scripts.Player.FSM.Locomotion
{
    public sealed class PlayerLocomotionFSM
    {
        public readonly Transform Owner;
        public readonly PlayerAnimationController Animation;
        
        public readonly LocomotionIdleState Idle;
        public readonly LocomotionWalkState Walk;
        public readonly LocomotionSprintState Sprint;
        
        private LocomotionBaseState m_Current;
        private readonly CharacterController m_Controller;
        private readonly Transform m_Camera;
        
        private readonly float m_WalkSpeed;
        private readonly float m_SprintSpeed;
        private float m_Yaw;

        // 튜닝 파라미터(모바일 감도/품질에 중요)
        private readonly float m_LookSensitivity;
        private readonly float m_TurnSharpness; // 0이면 즉시, >0이면 부드럽게

        public PlayerLocomotionFSM(
            Transform owner,
            CharacterController controller,
            Transform cameraTransform,
            PlayerAnimationController animation,
            float lookSensitivity,
            float turnSharpness,
            float walkSpeed,
            float sprintSpeed)
        {
            Owner = owner;
            m_Controller = controller;
            m_Camera = cameraTransform;
            Animation = animation;

            m_WalkSpeed = walkSpeed;
            m_SprintSpeed = sprintSpeed;

            m_LookSensitivity = lookSensitivity;
            m_TurnSharpness = turnSharpness;

            m_Yaw = Owner.eulerAngles.y;

            Idle = new LocomotionIdleState(this);
            Walk = new LocomotionWalkState(this);
            Sprint = new LocomotionSprintState(this);

            m_Current = Idle;
            m_Current.Enter();
        }

        public void Tick(in PlayerInputSnapshot input, float dt)
        {
            ApplyLookYaw(input.LookDelta.x, dt);
            
            m_Current.Tick(input, dt);
        }

        public void ChangeState(LocomotionBaseState next)
        {
            if (next == m_Current) return;
            Debug.Log($"Change state: {m_Current.GetType().Name} to {next.GetType().Name}");
            m_Current.Exit();
            m_Current = next;
            m_Current.Enter();
        }
        
        public void Move(in PlayerInputSnapshot input, float dt, float speed)
        {
            Vector2 move = input.Move;
            if (move.sqrMagnitude <= 0.0001f)
                return;

            // -1~1 입력 정규화(대각선 속도 보정)
            if (move.sqrMagnitude > 1f)
                move.Normalize();

            Vector3 forward = m_Camera != null ? m_Camera.forward : Owner.forward;
            Vector3 right   = m_Camera != null ? m_Camera.right   : Owner.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 worldDir = (right * move.x + forward * move.y);
            if (worldDir.sqrMagnitude > 1f) worldDir.Normalize();

            Vector3 delta = worldDir * speed * dt;

            if (m_Controller != null)
                m_Controller.Move(delta);
            else
                Owner.position += delta; // fallback
        }

        public float GetWalkSpeed() => m_WalkSpeed;
        public float GetSprintSpeed() => m_SprintSpeed;

        public void ApplyLookYaw(float lookDeltaX, float dt)
        {
            float yawDelta = lookDeltaX * m_LookSensitivity;
            if (Mathf.Abs(yawDelta) < 0.00001f) return;

            m_Yaw += yawDelta;

            Quaternion target = Quaternion.Euler(0f, m_Yaw, 0f);

            if (m_TurnSharpness <= 0f)
            {
                Owner.rotation = target;
                return;
            }

            // 프레임레이트 의존 줄이는 지수감쇠 보간
            float t = 1f - Mathf.Exp(-m_TurnSharpness * dt);
            Owner.rotation = Quaternion.Slerp(Owner.rotation, target, t);
        }
    }
}
