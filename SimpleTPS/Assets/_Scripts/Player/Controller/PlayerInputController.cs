using _Scripts.Player.Input;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player.Controller
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private PlayerInputUIBinder m_PlayerInputUIBinder;
        
        public Vector2 MoveDelta { get; private set; }
        public Vector2 LookDelta { get; private set; }

        public bool IsSprintPressed { get; private set; }
        public bool IsADSMode { get; private set; }
        public bool IsShootPressed { get; private set; }

        private bool m_IsRequestedReload;

        public void Awake()
        {
            m_PlayerInputUIBinder.Initialize(this);
        }
        
        // Locomotion
        private void OnMove(InputValue value) => SetMove(value.Get<Vector2>());
        private void OnLook(InputValue value) => SetLookDelta(value.Get<Vector2>());
        private void OnSprint(InputValue value) => SetSprintHeld(value.isPressed);
        
        public void SetMove(Vector2 move) => MoveDelta = move;
        public void SetLookDelta(Vector2 lookDelta) => LookDelta = lookDelta;
        public void SetSprintHeld(bool isPressed) => IsSprintPressed = isPressed;
        
        // Action
        private void OnAim(InputValue value)
        {
            if (value.isPressed) SetADSMode();
        }

        private void OnShoot(InputValue value)
        {
            SetShoot(value.isPressed);   
        }
        
        private void OnReload(InputValue value)
        {
            if (value.isPressed) RequestReload();
        }

        public void SetADSMode() => IsADSMode = !IsADSMode;
        public void SetShoot(bool isPressed) => IsShootPressed = isPressed;
        public void RequestReload() => m_IsRequestedReload = true;

        private bool IsReloadReady()
        {
            if (!m_IsRequestedReload) return false;
            m_IsRequestedReload = false;
            return true;
        }

        public PlayerInputSnapshot CaptureSnapshot() => new PlayerInputSnapshot
        (
            MoveDelta,
            LookDelta,
            IsSprintPressed,
            IsADSMode,
            IsShootPressed,
            IsReloadReady()
        );
    }
}