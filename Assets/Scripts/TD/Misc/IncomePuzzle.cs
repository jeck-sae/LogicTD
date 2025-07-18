using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public class IncomePuzzle : MonoBehaviour
    {
        public List<int> incomeLevels = new(); 
        [DisableInEditorMode, ShowInInspector] int currentLevel = 0;

        private void Start()
        {
            GameStats.Instance.SetCoinsPerSecond(incomeLevels[currentLevel]);
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
