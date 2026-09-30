using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewVersion.Fatia1.Scene
{
    public class NvSceneLevelVisibilityController : MonoBehaviour
    {
        [Serializable]
        private class LevelGroup
        {
            [SerializeField] private string levelId;
            [SerializeField] private GameObject[] roots;

            public string LevelId => levelId;
            public GameObject[] Roots => roots;
        }

        [Header("Scene Groups")]
        [SerializeField] private List<LevelGroup> levelGroups = new List<LevelGroup>();
        [SerializeField] private GameObject[] alwaysVisibleRoots;

        [Header("Startup")]
        [SerializeField] private bool hideGameplayGroupsOnAwake;

        [Header("Debug")]
        [SerializeField] private bool verboseLogs;

        private void Awake()
        {
            if (hideGameplayGroupsOnAwake)
            {
                HideAllGameplayGroups();
            }
            else
            {
                ApplyAlwaysVisibleRoots();
            }
        }

        public bool TryShowOnly(string levelId)
        {
            bool foundAny = false;

            for (int i = 0; i < levelGroups.Count; i++)
            {
                LevelGroup group = levelGroups[i];
                if (group == null)
                {
                    continue;
                }

                bool shouldBeActive = string.Equals(group.LevelId, levelId, StringComparison.OrdinalIgnoreCase);
                foundAny |= shouldBeActive;
                SetRootsActive(group.Roots, shouldBeActive);
            }

            ApplyAlwaysVisibleRoots();

            if (!foundAny)
            {
                Debug.LogWarning($"[NvSceneLevelVisibilityController] No scene group found for levelId '{levelId}'.", this);
            }
            else if (verboseLogs)
            {
                Debug.Log($"[NvSceneLevelVisibilityController] Active group: {levelId}", this);
            }

            return foundAny;
        }

        public void HideAllGameplayGroups()
        {
            for (int i = 0; i < levelGroups.Count; i++)
            {
                LevelGroup group = levelGroups[i];
                if (group == null)
                {
                    continue;
                }

                SetRootsActive(group.Roots, false);
            }

            ApplyAlwaysVisibleRoots();

            if (verboseLogs)
            {
                Debug.Log("[NvSceneLevelVisibilityController] All gameplay groups hidden.", this);
            }
        }

        public void ShowAllGameplayGroups()
        {
            for (int i = 0; i < levelGroups.Count; i++)
            {
                LevelGroup group = levelGroups[i];
                if (group == null)
                {
                    continue;
                }

                SetRootsActive(group.Roots, true);
            }

            ApplyAlwaysVisibleRoots();

            if (verboseLogs)
            {
                Debug.Log("[NvSceneLevelVisibilityController] All gameplay groups shown.", this);
            }
        }

        private void ApplyAlwaysVisibleRoots()
        {
            SetRootsActive(alwaysVisibleRoots, true);
        }

        private static void SetRootsActive(GameObject[] roots, bool isActive)
        {
            if (roots == null)
            {
                return;
            }

            for (int i = 0; i < roots.Length; i++)
            {
                GameObject root = roots[i];
                if (root == null)
                {
                    continue;
                }

                if (root.activeSelf != isActive)
                {
                    root.SetActive(isActive);
                }
            }
        }
    }
}
