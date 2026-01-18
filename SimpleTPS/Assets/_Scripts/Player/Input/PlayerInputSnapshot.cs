using UnityEngine;

namespace _Scripts.Player.Input
{
    public readonly struct PlayerInputSnapshot
    {
        public readonly Vector2 Move;
        public readonly Vector2 LookDelta;

        public readonly bool SprintHeld;
        public readonly bool AimHeld;
        public readonly bool ShootHeld;
        public readonly bool ReloadPressed;

        public PlayerInputSnapshot(
            Vector2 move,
            Vector2 lookDelta,
            bool sprintHeld,
            bool aimHeld,
            bool shootHeld,
            bool reloadPressed)
        {
            Move = move;
            LookDelta = lookDelta;

            SprintHeld = sprintHeld;
            AimHeld = aimHeld;
            ShootHeld = shootHeld;
            ReloadPressed = reloadPressed;
        }
    }
}