using _Scripts.Player.Controller;
using _Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Player.Input
{
    public sealed class PlayerInputUIBinder : MonoBehaviour
    {
        [SerializeField] private UIHoldButton m_LeftShootButton;
        [SerializeField] private UIHoldButton m_RightShootButton;
        [SerializeField] private UITapButton  m_ReloadButton;
        [SerializeField] private UIToggleButton  m_AimButton;
        [SerializeField] private Joystick m_MoveJoystick;
        [SerializeField] private UITouchPanel m_LookPanel;
        
        private PlayerInputController m_PlayerInputController;

        public void Initialize(PlayerInputController playerInputController)
        {
            m_PlayerInputController = playerInputController;
            Bind();
        } 

        private void OnDisable() => Unbind();

        private void Bind()
        {
            if (m_PlayerInputController == null) return;

            if (m_LeftShootButton != null) m_LeftShootButton.PressedChanged += OnShootPressedChanged;
            if (m_RightShootButton != null) m_RightShootButton.PressedChanged += OnShootPressedChanged;
            if (m_ReloadButton != null) m_ReloadButton.Clicked += OnReloadClicked;
            if (m_AimButton != null) m_AimButton.Clicked += OnAimClicked;
            if (m_MoveJoystick != null) m_MoveJoystick.OnJoystickDirection += OnMoveJoystickDirection;
            if (m_LookPanel != null) m_LookPanel.OnLookDelta += OnLookDelta;
        }

        private void Unbind()
        {
            if (m_LeftShootButton != null) m_LeftShootButton.PressedChanged -= OnShootPressedChanged;
            if (m_RightShootButton != null) m_RightShootButton.PressedChanged -= OnShootPressedChanged;
            if (m_ReloadButton != null) m_ReloadButton.Clicked -= OnReloadClicked;
            if (m_AimButton != null) m_AimButton.Clicked -= OnAimClicked;
            if (m_MoveJoystick != null) m_MoveJoystick.OnJoystickDirection -= OnMoveJoystickDirection;
            if (m_LookPanel != null) m_LookPanel.OnLookDelta -= OnLookDelta;
        }

        private void OnShootPressedChanged(bool pressed) => m_PlayerInputController.SetShoot(pressed);

        private void OnReloadClicked() => m_PlayerInputController.RequestReload();

        private void OnAimClicked() => m_PlayerInputController.SetADSMode();
        
        private void OnMoveJoystickDirection(Vector2 dir) => m_PlayerInputController.SetMove(dir);
        
        private void OnLookDelta(Vector2 delta) => m_PlayerInputController.SetLookDelta(delta);
    }
}