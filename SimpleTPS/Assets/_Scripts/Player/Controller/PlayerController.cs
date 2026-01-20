using _Scripts.Player.FSM.Action;
using _Scripts.Player.FSM.Locomotion;
using _Scripts.Player.Input;
using UnityEngine;

namespace _Scripts.Player.Controller
{   
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerAnimationController))]
    public class PlayerController : MonoBehaviour
    {  
        private CharacterController m_CharacterController;
        private PlayerInputController m_PlayerInputController;
        private PlayerAnimationController m_PlayerAnimationController;
        private PlayerAimController m_PlayerAimController;
        private PlayerStatController m_PlayerStatController;
        private PlayerWeaponController m_PlayerWeaponController;
        private PlayerInputSnapshot m_LastSnapshot;
        
        private PlayerLocomotionFSM m_LocomotionFSM;
        private PlayerActionFSM m_PlayerActionFSM;
        
        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_PlayerInputController = GetComponent<PlayerInputController>();
            m_PlayerAnimationController = GetComponent<PlayerAnimationController>();
            m_PlayerAimController = GetComponent<PlayerAimController>();
            m_PlayerStatController = GetComponent<PlayerStatController>();
            m_PlayerWeaponController = GetComponent<PlayerWeaponController>();
            
            m_LocomotionFSM = new PlayerLocomotionFSM(
                m_CharacterController,
                m_PlayerAimController,
                m_PlayerAnimationController,
                m_PlayerStatController
            );

            m_PlayerActionFSM = new PlayerActionFSM(m_PlayerAnimationController);
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
            m_PlayerActionFSM.Tick(m_LastSnapshot, Time.deltaTime);
            m_PlayerAimController.Tick(m_LastSnapshot);
            m_PlayerWeaponController.Tick(m_LastSnapshot, Time.deltaTime);
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