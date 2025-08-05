using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public class IncomePuzzleManager : MonoBehaviour
    {
        public int startIncome = 1;
        [SerializeReference] public List<PuzzleLevel> levels = new ();
        [ReadOnly, ShowInInspector] int currentLevel = 0;

        private GameObject puzzleObj;
        private LogicPuzzle puzzle;
        
        private void Start()
        {
            GameStats.Instance.SetCoinsPerSecond(startIncome);
            SpawnPuzzle(0);
        }

        private void OnDestroy()
        {
            if (puzzle)
                puzzle.OnPuzzleSolved -= OnPuzzleSolved;
        }

        public void OnPuzzleSolved()
        {
            NextLevel();
        }

        [Button, DisableInEditorMode]
        public void NextLevel()
        {
            if (currentLevel >= levels.Count)
                return;
            
            GameStats.Instance.SetCoinsPerSecond(levels[currentLevel].incomeReward);
            
            currentLevel++;
            SpawnPuzzle(currentLevel);
        }

        protected void SpawnPuzzle(int level)
        {
            if(puzzle) puzzle.OnPuzzleSolved -= OnPuzzleSolved;
            if(puzzleObj) Destroy(puzzleObj);
            if(level >= levels.Count) return;
            
            puzzleObj = Instantiate(levels[level].puzzlePrefab, transform);
            puzzleObj.transform.localPosition = Vector3.zero;
            puzzle = puzzleObj.GetComponent<LogicPuzzle>();
            puzzle.OnPuzzleSolved += OnPuzzleSolved;
            LogicManager.Instance.RefreshNodes();
        }

        [Serializable]
        public class PuzzleLevel
        {
            public int incomeReward;
            public GameObject puzzlePrefab;
        }
    }
}
