using UnityEngine;

namespace NewVersion.Fatia1.LegacyBridge
{
    public class NvLegacyLevelRunnerAdapter : MonoBehaviour
    {
        [SerializeField] private InterfaceManager legacyInterfaceManager;
        [SerializeField] private bool verboseLogs;

        public bool IsConfigured => legacyInterfaceManager != null;

        public void OpenLevelSelection()
        {
            if (!EnsureConfigured())
            {
                return;
            }

            legacyInterfaceManager._TurnOnLevelSelection();
        }

        public void CloseLevelSelection()
        {
            if (!EnsureConfigured())
            {
                return;
            }

            legacyInterfaceManager.OffSelectLevel();
        }

        public void RunLegacyLevelSlot(int slot)
        {
            if (!EnsureConfigured())
            {
                return;
            }

            switch (slot)
            {
                case 1:
                    legacyInterfaceManager.Playlevel1();
                    break;
                case 2:
                    legacyInterfaceManager.Playlevel2();
                    break;
                case 3:
                    legacyInterfaceManager.PlayLevel3();
                    break;
                case 4:
                    legacyInterfaceManager.PlayLevel4();
                    break;
                default:
                    Debug.LogWarning($"[NvLegacyLevelRunnerAdapter] Unsupported legacy slot: {slot}", this);
                    break;
            }

            if (verboseLogs)
            {
                Debug.Log($"[NvLegacyLevelRunnerAdapter] RunLegacyLevelSlot({slot})", this);
            }
        }

        public void ReplayCurrentLevel()
        {
            if (!EnsureConfigured())
            {
                return;
            }

            legacyInterfaceManager.Reiniciar(2);

            if (verboseLogs)
            {
                Debug.Log("[NvLegacyLevelRunnerAdapter] ReplayCurrentLevel()", this);
            }
        }

        public void ReturnToMainMenu()
        {
            if (!EnsureConfigured())
            {
                return;
            }

            legacyInterfaceManager.Reiniciar(1);

            if (verboseLogs)
            {
                Debug.Log("[NvLegacyLevelRunnerAdapter] ReturnToMainMenu()", this);
            }
        }

        private bool EnsureConfigured()
        {
            if (legacyInterfaceManager != null)
            {
                return true;
            }

            Debug.LogError("[NvLegacyLevelRunnerAdapter] Missing InterfaceManager reference.", this);
            return false;
        }
    }
}
