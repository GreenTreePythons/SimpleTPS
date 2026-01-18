using _Scripts.Player.FSM.Locomotion;
using _Scripts.Player.Input;
using UnityEngine;

namespace _Scripts.Player.Controller
{
    public enum PlayerStateType
    {
        Idle, Walk, Sprint, Reload, Shoot, Aim
    }
    
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerAnimationController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float m_LookSensitivity = 0.1f;
        [SerializeField] private float m_TurnSharpness = 0f;
        [SerializeField] private float m_RunSpeed = 2f;
        [SerializeField] private float m_WalkSpeed = 1f;
        
        private CharacterController m_CharacterController;
        private PlayerInputController m_PlayerInputController;
        private PlayerAnimationController m_PlayerAnimationController;
        private PlayerInputSnapshot m_LastSnapshot;
        
        private PlayerLocomotionFSM m_LocomotionFSM;

        private Transform m_CameraTransform;
        
        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_PlayerInputController = GetComponent<PlayerInputController>();
            m_PlayerAnimationController = GetComponent<PlayerAnimationController>();
            m_CameraTransform = Camera.main.transform;
            
            m_LocomotionFSM = new PlayerLocomotionFSM(
                transform,
                m_CharacterController,
                m_CameraTransform,
                m_PlayerAnimationController,
                m_LookSensitivity,
                m_TurnSharpness,
                m_WalkSpeed,
                m_RunSpeed
            );
        }

        private void OnEnable()
        {
            
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            m_LastSnapshot = m_PlayerInputController.CaptureSnapshot();
            m_LocomotionFSM.Tick(m_LastSnapshot, Time.deltaTime);
        }

        private void LateUpdate()
        {
            
        }

        private void FixedUpdate()
        {
            
        }

        private void OnDisable()
        {
            
        }
    }
}