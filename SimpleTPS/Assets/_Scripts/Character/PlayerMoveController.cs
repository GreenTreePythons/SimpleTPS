// using System;
// using UnityEngine;
//
// namespace _Scripts.Character
// {   
//     [RequireComponent(typeof(CharacterController))]
//     public sealed class PlayerMoveController : MonoBehaviour
//     {
//         [Header("Stat")]
//         [SerializeField] float m_MoveSpeed = 2.0f;
//         
//         [Header("Settings")]
//         [SerializeField] float m_RotateSpeedDegPerSec = 720f;
//
//         private CharacterController m_CharacterController;
//         private Transform m_CameraTransform;
//         private PlayerStateType m_CurrentPlayerLocomotionType;
//         private bool m_IsLocoMotionUpdate;
//
//         public Vector3 WorldMoveDirection { get; private set; }
//         public event Action<PlayerStateType> OnLocomotionChanged;
//
//         public void Initialize(CharacterController cc)
//         {
//             m_CharacterController = cc;
//             m_CameraTransform = Camera.main.transform;
//             m_CurrentPlayerLocomotionType = PlayerStateType.Idle;
//             UpdateLocoMotion(false);
//         }
//
//         public void OnUpdate()
//         {
//             if (!m_IsLocoMotionUpdate) return;
//             
//             OnLocomotionChanged?.Invoke(m_CurrentPlayerLocomotionType);
//             m_IsLocoMotionUpdate = false;
//         }
//
//         public void OnFixedUpdate(Vector2 input, float deltaTime)
//         {
//             Vector3 forward = m_CameraTransform.forward;
//             forward.y = 0f;
//             Vector3 right = m_CameraTransform.right;
//             right.y = 0f;
//
//             Vector3 worldDir = right * input.x + forward * input.y;
//             worldDir = Vector3.ClampMagnitude(worldDir, 1f);
//             
//             var isMoving = worldDir.sqrMagnitude > 0.0001f;
//             
//             UpdateLocoMotion(isMoving);
//             
//             if (!isMoving) return; 
//             
//             WorldMoveDirection = worldDir;
//             
//             Quaternion targetRot = Quaternion.LookRotation(worldDir, Vector3.up);
//             transform.rotation = Quaternion.RotateTowards(
//                 transform.rotation,
//                 targetRot,
//                 m_RotateSpeedDegPerSec * deltaTime
//             );
//
//             m_CharacterController.Move(worldDir * (m_MoveSpeed * deltaTime));
//         }
//
//         private void UpdateLocoMotion(bool isMoving)
//         {
//             var next = isMoving ? PlayerStateType.Move : PlayerStateType.Idle;
//             if (next == m_CurrentPlayerLocomotionType) return;
//             
//             m_CurrentPlayerLocomotionType = next;
//             m_IsLocoMotionUpdate = true;
//         }
//     }
// }
