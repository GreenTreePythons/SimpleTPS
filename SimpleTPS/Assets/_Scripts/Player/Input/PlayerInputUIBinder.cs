using _Scripts.Player.Controller;
using UnityEngine;

namespace _Scripts.Player.Input
{
    public sealed class PlayerInputUIBinder : MonoBehaviour
    {
        private PlayerInputController m_PlayerInputController;

        public void Initialize(PlayerInputController playerInputController)
        {
            m_PlayerInputController = playerInputController;
        }

        private void Awake()
        {
            // TODO: 추후 UI 위젯(조이스틱/버튼)을 바인딩할 때 사용.
            // 예: Shoot 버튼 pressed 이벤트 -> m_PlayerInputController.SetShootHeld(...)
            // 예: Aim 버튼 pressed 이벤트 -> m_PlayerInputController.SetAimHeld(...)
            // 예: Reload 버튼 click 이벤트 -> m_PlayerInputController.RequestReload()
        }
    }
}