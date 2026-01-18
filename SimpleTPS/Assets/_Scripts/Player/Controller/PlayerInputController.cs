using _Scripts.Player.Input;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player.Controller
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputController : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public Vector2 LookDelta { get; private set; }

        public bool SprintHeld { get; private set; }
        public bool AimHeld { get; private set; }
        public bool ShootHeld { get; private set; }

        private bool m_IsReloadReady;
        
        // Locomotion
        private void OnMove(InputValue value) => Move = value.Get<Vector2>();
        private void OnLook(InputValue value) => LookDelta = value.Get<Vector2>();
        private void OnSprint(InputValue value) => SprintHeld = value.isPressed;
        
        public void SetMove(Vector2 move) => Move = move;
        public void SetLookDelta(Vector2 lookDelta) => LookDelta = lookDelta;
        public void SetSprintHeld(bool held) => SprintHeld = held;
        
        // Action
        private void OnAim(InputValue value) => AimHeld = value.isPressed;
        private void OnShoot(InputValue value) => ShootHeld = value.isPressed;
        private void OnReload(InputValue value)
        {
            if (value.isPressed) RequestReload();
        }

        public void SetAimHeld(bool held) => AimHeld = held;
        public void SetShootHeld(bool held) => ShootHeld = held;
        public void RequestReload() => m_IsReloadReady = true;

        private bool ReloadUpdated()
        {
            if (!m_IsReloadReady) return false;
            m_IsReloadReady = false;
            return true;
        }

        public PlayerInputSnapshot CaptureSnapshot() => new PlayerInputSnapshot
        (
            Move,
            LookDelta,
            SprintHeld,
            AimHeld,
            ShootHeld,
            ReloadUpdated()
        );
    }
}