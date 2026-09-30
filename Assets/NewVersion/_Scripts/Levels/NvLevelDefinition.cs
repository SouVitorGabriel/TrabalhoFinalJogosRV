using UnityEngine;

namespace NewVersion.Fatia1.Levels
{
    [CreateAssetMenu(
        fileName = "NvLevelDefinition",
        menuName = "NewVersion/Fatia1/Level Definition",
        order = 1)]
    public class NvLevelDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string levelId = "level-1";
        [SerializeField] private string displayName = "Level 1";

        [Header("Legacy Bridge")]
        [Range(1, 4)]
        [SerializeField] private int legacyLevelSlot = 1;

        [Header("Future Data (not used yet)")]
        [SerializeField] private bool usePlayerSpawnOverride;
        [SerializeField] private Vector3 playerSpawnOverride;
        [SerializeField] private bool enableEnemy = true;
        [SerializeField] private string notes;

        public string LevelId => levelId;
        public string DisplayName => displayName;
        public int LegacyLevelSlot => legacyLevelSlot;
        public bool UsePlayerSpawnOverride => usePlayerSpawnOverride;
        public Vector3 PlayerSpawnOverride => playerSpawnOverride;
        public bool EnableEnemy => enableEnemy;
        public string Notes => notes;
    }
}
