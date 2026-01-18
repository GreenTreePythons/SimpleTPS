using UnityEngine;

namespace _Scripts.Player.Input
{
    public readonly struct PlayerInputSnapshot
    {
        public readonly Vector2 Move;
        public readonly Vector2 LookDelta;

        public readonly bool IsSprintPressed;
        public readonly bool IsADSMode;
        public readonly bool IsShootPressed;
        public readonly bool IsReloadPressed;

        public PlayerInputSnapshot(
            Vector2 move,
            Vector2 lookDelta,
            bool isSprintPressed,
            bool isADSMode,
            bool isShootPressed,
            bool isReloadPressed)
        {
            Move = move;
            LookDelta = lookDelta;

            IsSprintPressed = isSprintPressed;
            IsADSMode = isADSMode;
            IsShootPressed = isShootPressed;
            IsReloadPressed = isReloadPressed;
        }
    }
}