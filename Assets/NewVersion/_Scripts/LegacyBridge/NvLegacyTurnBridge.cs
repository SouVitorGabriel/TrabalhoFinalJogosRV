using NewVersion.Fatia1.Turns;
using UnityEngine;

namespace NewVersion.Fatia1.LegacyBridge
{
    public class NvLegacyTurnBridge : MonoBehaviour
    {
        [SerializeField] private MovementController player;
        [SerializeField] private EnemyMovementController enemy;
        [SerializeField] private bool driveEnemy = true;
        [SerializeField] private bool verboseLogs;

        public bool TryIssueTurn(NvTurnDirection direction)
        {
            if (direction == NvTurnDirection.None)
            {
                return false;
            }

            if (player == null)
            {
                Debug.LogError("[NvLegacyTurnBridge] Missing player MovementController.", this);
                return false;
            }

            ApplyToPlayer(direction);

            if (driveEnemy && enemy != null)
            {
                ApplyToEnemy(direction);
            }

            if (verboseLogs)
            {
                Debug.Log($"[NvLegacyTurnBridge] Issued turn: {direction}", this);
            }

            return true;
        }

        private void ApplyToPlayer(NvTurnDirection direction)
        {
            switch (direction)
            {
                case NvTurnDirection.Up:
                    player.frente = true;
                    break;
                case NvTurnDirection.Down:
                    player.atras = true;
                    break;
                case NvTurnDirection.Left:
                    player.esquerda = true;
                    break;
                case NvTurnDirection.Right:
                    player.direita = true;
                    break;
            }
        }

        private void ApplyToEnemy(NvTurnDirection direction)
        {
            // Mirror profile kept equal to legacy SwipeManager behavior:
            // horizontal follows player, vertical is inverted.
            switch (direction)
            {
                case NvTurnDirection.Up:
                    enemy.atras = true;
                    break;
                case NvTurnDirection.Down:
                    enemy.frente = true;
                    break;
                case NvTurnDirection.Left:
                    enemy.esquerda = true;
                    break;
                case NvTurnDirection.Right:
                    enemy.direita = true;
                    break;
            }
        }
    }
}
