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
        [SerializeField] private Image m_AimImage;
        [SerializeField] private GameObject m_AimTargetObj;
        [SerializeField] private float m_AimDistance = 10f;
        [SerializeField] private CinemachineCamera m_DefaultCamera;
        [SerializeField] private CinemachineCamera m_ShoulderCamera;
        [SerializeField] private CinemachineCamera m_ADSCamera;
        
        private Transform m_CameraTransform;
        private PlayerAimType m_PlayerAimType;
        private PlayerInputSnapshot m_PlayerInput;
        private bool m_IsAimHitted;

        private void Awake()
        {
            m_CameraTransform = m_DefaultCamera.transform;
            m_PlayerAimType = PlayerAimType.Default;
        }
        
        public void Tick(PlayerInputSnapshot playerInput)
        {
            m_PlayerInput = playerInput;
            UpdateCamera();

            bool isAimHit = Physics.Raycast(m_CameraTransform.position, m_CameraTransform.forward, out RaycastHit hit, m_AimDistance);
            m_IsAimHitted = isAimHit;
            UpdateAimImage(isAimHit);
            UpdateAimTarget(isAimHit, hit);
        }

        private void UpdateCamera()
        {
            if (m_PlayerInput.IsADSMode) m_PlayerAimType = PlayerAimType.ADS;
            else if (m_PlayerInput.IsShootPressed) m_PlayerAimType = PlayerAimType.Shoulder;
            else m_PlayerAimType = PlayerAimType.Default;

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

        private void UpdateAimImage(bool isAimHit)
        {
            if (m_IsAimHitted == isAimHit) return;
            m_AimImage.color = isAimHit ? Color.red : Color.white;
        }

        private void UpdateAimTarget(bool isAimHit, RaycastHit hit)
        {
            Vector3 targetPos = isAimHit
                ? hit.point
                : (m_CameraTransform.position + m_CameraTransform.forward * m_AimDistance);
            m_AimTargetObj.transform.position = targetPos;
        }
        
        public Transform GetCameraTransform() => m_CameraTransform;
    }
}