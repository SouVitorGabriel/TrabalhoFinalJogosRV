using System.Collections.Generic;
using System;
using UnityEngine;

namespace NewVersion.Fatia1.Levels
{
    [CreateAssetMenu(
        fileName = "NvLevelCatalog",
        menuName = "NewVersion/Fatia1/Level Catalog",
        order = 2)]
    public class NvLevelCatalog : ScriptableObject
    {
        [SerializeField] private List<NvLevelDefinition> levels = new List<NvLevelDefinition>();

        public int Count => levels.Count;

        public bool TryGetLevel(int index, out NvLevelDefinition level)
        {
            if (index < 0 || index >= levels.Count)
            {
                level = null;
                return false;
            }

            level = levels[index];
            return level != null;
        }

        public bool TryGetLevelByLegacySlot(int legacySlot, out NvLevelDefinition level)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                NvLevelDefinition candidate = levels[i];
                if (candidate != null && candidate.LegacyLevelSlot == legacySlot)
                {
                    level = candidate;
                    return true;
                }
            }

            level = null;
            return false;
        }

        public bool TryGetLevelById(string levelId, out NvLevelDefinition level, out int index)
        {
            if (string.IsNullOrWhiteSpace(levelId))
            {
                level = null;
                index = -1;
                return false;
            }

            for (int i = 0; i < levels.Count; i++)
            {
                NvLevelDefinition candidate = levels[i];
                if (candidate == null)
                {
                    continue;
                }

                if (string.Equals(candidate.LevelId, levelId, StringComparison.OrdinalIgnoreCase))
                {
                    level = candidate;
                    index = i;
                    return true;
                }
            }

            level = null;
            index = -1;
            return false;
        }
    }
}
