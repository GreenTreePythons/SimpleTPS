using System;
using _Scripts.Player.Input;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.Player.Controller
{
    public enum PlayerAimType
    {
        Default, Shoulder, ADS
    }
    
    public class PlayerAimController : MonoBehaviour
    {
        [SerializeField] private Transform m_PlayerTransform;
        [SerializeField] private Transform m_CameraPivotTransfrom;
        [SerializeField] private Image m_AimImage;
        [SerializeField] private GameObject m_AimTargetObj;
        [SerializeField] private float m_AimDistance = 10f;
        
        [SerializeField] private float m_DefaultLookSpeed = 0.1f;
        [SerializeField] private float m_ShoulderLookSpeed = 0.1f;
        [SerializeField] private float m_ADSLookSpeed = 0.1f;
        
        [SerializeField] private float m_MinPitch = -60f;
        [SerializeField] private float m_MaxPitch = 75f;
        [SerializeField] private CinemachineCamera m_DefaultCamera;
        [SerializeField] private CinemachineCamera m_ShoulderCamera;
        [SerializeField] private CinemachineCamera m_ADSCamera;
        
        private Transform m_CameraTransform;
        private float m_Yaw;
        private float m_Pitch;
        private PlayerAimType m_PlayerAimType;
        private PlayerInputSnapshot m_PlayerInput;

        private void Awake()
        {
            m_CameraTransform = m_DefaultCamera.transform;
            m_PlayerAimType = PlayerAimType.Default;
        }
        
        public void Tick(PlayerInputSnapshot playerInput)
        {
            m_PlayerInput = playerInput;
            UpdateCameraType();
            UpdateAimTarget();
            UpdateAim(playerInput.LookDelta);
        }

        private void UpdateAim(Vector2 lookDelta)
        {
            var sensitivity = GetAimSensitivityByCamera();
            m_Yaw += lookDelta.x * sensitivity;
            
            float deltaY = lookDelta.y * sensitivity;
            m_Pitch = Mathf.Clamp(m_Pitch - deltaY, m_MinPitch, m_MaxPitch);
            
            m_PlayerTransform.rotation = Quaternion.Euler(0f, m_Yaw, 0f);
            m_CameraPivotTransfrom.rotation = Quaternion.Euler(m_Pitch, m_Yaw, 0f);
        }

        private void UpdateCameraType()
        {
            var playerAimTarget = PlayerAimType.Default;
            if (m_PlayerInput.IsADSMode) playerAimTarget = PlayerAimType.ADS;
            else if (m_PlayerInput.IsShootPressed) playerAimTarget = PlayerAimType.Shoulder;

            if (m_PlayerAimType == playerAimTarget) return;
            m_PlayerAimType = playerAimTarget;

            m_DefaultCamera.Priority = 0;
            m_ShoulderCamera.Priority = 0;
            m_ADSCamera.Priority = 0;

            switch (m_PlayerAimType)
            {
                case PlayerAimType.ADS:
                    m_ADSCamera.Priority = 1;
                    m_CameraTransform = m_ADSCamera.transform;
                    break;

                case PlayerAimType.Shoulder:
                    m_ShoulderCamera.Priority = 1;
                    m_CameraTransform = m_ShoulderCamera.transform;
                    break;

                default:
                    m_DefaultCamera.Priority = 1;
                    m_CameraTransform = m_DefaultCamera.transform;
                    break;
            }
        }

        private void UpdateAimTarget()
        {
            var isHitted = Physics.Raycast(m_CameraTransform.position, m_CameraTransform.forward, out RaycastHit hit,
                m_AimDistance);
            
            Vector3 targetPos = default;
            if (isHitted)
            {
                var hittedObj = hit.collider.gameObject;
                if (hittedObj.layer == LayerMask.NameToLayer("Enviroment"))
                {
                    m_AimImage.color = Color.blue;    
                }
                else if (hittedObj.layer == LayerMask.NameToLayer("Enemy"))
                {
                    m_AimImage.color = Color.red;
                }
                targetPos = hit.point;
            }
            else
            {
                m_AimImage.color = Color.white;
                targetPos = m_CameraTransform.position + m_CameraTransform.forward * m_AimDistance;
            }
            
            m_AimTargetObj.transform.position = targetPos;
        }
        
        private float GetAimSensitivityByCamera() => m_PlayerAimType switch
        {
            PlayerAimType.Default => m_DefaultLookSpeed,
            PlayerAimType.Shoulder => m_ShoulderLookSpeed,
            PlayerAimType.ADS => m_ADSLookSpeed,
            _ => m_DefaultLookSpeed
        };
        
        public Transform GetCameraTransform() => m_CameraTransform;
    }
}