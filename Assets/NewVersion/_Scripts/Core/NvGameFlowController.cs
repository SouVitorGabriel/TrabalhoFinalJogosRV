using NewVersion.Fatia1.LegacyBridge;
using NewVersion.Fatia1.Levels;
using NewVersion.Fatia1.Scene;
using UnityEngine;

namespace NewVersion.Fatia1.Core
{
    public class NvGameFlowController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private NvLevelCatalog levelCatalog;
        [SerializeField] private NvLegacyLevelRunnerAdapter legacyRunner;
        [SerializeField] private NvSceneLevelVisibilityController sceneVisibility;

        [Header("Startup")]
        [SerializeField] private bool autoEnterMainMenuOnStart = true;
        [SerializeField] private bool controlSceneVisibility;
        [SerializeField] private bool hideGameplayGroupsOnMainMenu = true;

        [Header("Runtime")]
        [SerializeField] private NvGameState currentState = NvGameState.Boot;
        [SerializeField] private int currentLevelIndex = -1;

        private NvLevelDefinition currentLevel;

        public NvGameState CurrentState => currentState;
        public int CurrentLevelIndex => currentLevelIndex;
        public NvLevelDefinition CurrentLevel => currentLevel;

        private void Awake()
        {
            SetState(NvGameState.Boot);
        }

        private void Start()
        {
            if (autoEnterMainMenuOnStart)
            {
                EnterMainMenu();
            }
        }

        public void EnterMainMenu()
        {
            if (controlSceneVisibility && sceneVisibility != null)
            {
                if (hideGameplayGroupsOnMainMenu)
                {
                    sceneVisibility.HideAllGameplayGroups();
                }
                else
                {
                    sceneVisibility.ShowAllGameplayGroups();
                }
            }

            SetState(NvGameState.MainMenu);
        }

        public void OpenLevelSelection()
        {
            if (legacyRunner != null)
            {
                legacyRunner.OpenLevelSelection();
            }

            SetState(NvGameState.LevelSelection);
        }

        public void CloseLevelSelection()
        {
            if (legacyRunner != null)
            {
                legacyRunner.CloseLevelSelection();
            }

            SetState(NvGameState.MainMenu);
        }

        public void PlayCatalogLevel(int levelIndex)
        {
            if (levelCatalog == null)
            {
                Debug.LogError("[NvGameFlowController] Missing level catalog.", this);
                return;
            }

            if (!levelCatalog.TryGetLevel(levelIndex, out NvLevelDefinition definition))
            {
                Debug.LogError($"[NvGameFlowController] Invalid level index: {levelIndex}", this);
                return;
            }

            if (legacyRunner == null)
            {
                Debug.LogError("[NvGameFlowController] Missing legacy runner adapter.", this);
                return;
            }

            currentLevelIndex = levelIndex;
            currentLevel = definition;

            if (controlSceneVisibility && sceneVisibility != null)
            {
                bool visibleGroupFound = sceneVisibility.TryShowOnly(currentLevel.LevelId);
                if (!visibleGroupFound)
                {
                    Debug.LogWarning($"[NvGameFlowController] No scene group for levelId '{currentLevel.LevelId}'. Showing all gameplay groups as fallback.", this);
                    sceneVisibility.ShowAllGameplayGroups();
                }
            }

            SetState(NvGameState.Transition);
            legacyRunner.RunLegacyLevelSlot(definition.LegacyLevelSlot);
            SetState(NvGameState.Playing);
        }

        public void PlayLevelById(string levelId)
        {
            if (levelCatalog == null)
            {
                Debug.LogError("[NvGameFlowController] Missing level catalog.", this);
                return;
            }

            if (!levelCatalog.TryGetLevelById(levelId, out NvLevelDefinition definition, out int index))
            {
                Debug.LogError($"[NvGameFlowController] Unknown level id: {levelId}", this);
                return;
            }

            PlayCatalogLevel(index);
        }

        public void PlayLevel1()
        {
            PlayCatalogLevel(0);
        }

        public void PlayLevel2()
        {
            PlayCatalogLevel(1);
        }

        public void PlayLevel3()
        {
            PlayCatalogLevel(2);
        }

        public void PlayLevel4()
        {
            PlayCatalogLevel(3);
        }

        public void ReplayCurrentLevel()
        {
            if (legacyRunner == null)
            {
                Debug.LogError("[NvGameFlowController] Missing legacy runner adapter.", this);
                return;
            }

            if (currentLevel != null)
            {
                if (controlSceneVisibility && sceneVisibility != null)
                {
                    bool visibleGroupFound = sceneVisibility.TryShowOnly(currentLevel.LevelId);
                    if (!visibleGroupFound)
                    {
                        Debug.LogWarning($"[NvGameFlowController] No scene group for levelId '{currentLevel.LevelId}'. Showing all gameplay groups as fallback.", this);
                        sceneVisibility.ShowAllGameplayGroups();
                    }
                }

                SetState(NvGameState.Transition);
                legacyRunner.RunLegacyLevelSlot(currentLevel.LegacyLevelSlot);
                SetState(NvGameState.Playing);
                return;
            }

            legacyRunner.ReplayCurrentLevel();
            SetState(NvGameState.Playing);
        }

        public void ReturnToMainMenu()
        {
            if (legacyRunner != null)
            {
                legacyRunner.ReturnToMainMenu();
            }

            EnterMainMenu();
        }

        public void MarkWin()
        {
            SetState(NvGameState.Won);
        }

        public void MarkLose()
        {
            SetState(NvGameState.Lost);
        }

        // Legacy callback aliases let us retarget old UI buttons to this controller
        // without redesigning all button signatures right now.
        public void _TurnOnLevelSelection()
        {
            OpenLevelSelection();
        }

        public void OffSelectLevel()
        {
            CloseLevelSelection();
        }

        public void Playlevel1()
        {
            PlayLevel1();
        }

        public void Playlevel2()
        {
            PlayLevel2();
        }

        public void Reiniciar()
        {
            Reiniciar(0);
        }

        public void Reiniciar(int mode)
        {
            if (mode == 2)
            {
                ReplayCurrentLevel();
                return;
            }

            ReturnToMainMenu();
        }

        private void SetState(NvGameState nextState)
        {
            currentState = nextState;
        }
    }
}
