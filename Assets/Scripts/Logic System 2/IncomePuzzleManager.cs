using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public class IncomePuzzleManager : MonoBehaviour
    {
        public List<int> incomeLevels = new();
        [ShowInInspector] public List<(int incomeReward, GameObject puzzlePrefab)> levels;
        [DisableInEditorMode, ShowInInspector] int currentLevel = 0;

        
        
        private void Start()
        {
            GameStats.Instance.SetCoinsPerSecond(incomeLevels[currentLevel]);
            LogicManager.Instance.OnStatesUpdated += CheckResults;
        }

        private void OnDestroy()
        {
            if (LogicManager.Instance)
                LogicManager.Instance.OnStatesUpdated -= CheckResults;
        }

        public void CheckResults()
        {
            var puzzle = FindAnyObjectByType<LogicPuzzle>();
            
            if(puzzle && puzzle.CheckResult())
                NextLevel();
        }

        [Button, DisableInEditorMode]
        public void NextLevel()
        {
            currentLevel++;
            
            // Max Level
            if (currentLevel >= incomeLevels.Count)
                return;
            
            GameStats.Instance.SetCoinsPerSecond(incomeLevels[currentLevel]);
        }
    }
}
