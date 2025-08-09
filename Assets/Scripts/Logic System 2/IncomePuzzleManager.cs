using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class IncomePuzzleManager : MonoBehaviour
    {
        public int startIncome = 1;
        [SerializeReference] public List<PuzzleLevel> levels = new ();
        [ReadOnly, ShowInInspector] int currentLevel = 0;
        public GameObject nextPuzzleButton;
        public Slider loadingPuzzleSlider;
        
        public int delayBetweenPuzzles = 15;
        
        private GameObject puzzleObj;
        private LogicPuzzle puzzle;
        
        private void Start()
        {
            nextPuzzleButton.SetActive(false);            
            loadingPuzzleSlider.gameObject.SetActive(false);

            GameStats.Instance.SetCoinsPerSecond(startIncome);
            SpawnPuzzle(0);
        }

        private void OnDestroy()
        {
            if (puzzle) puzzle.OnPuzzleSolved -= OnPuzzleSolved;
            if (puzzle) puzzle.OnPuzzleUnsolved -= OnPuzzleUnsolved;
        }

        public void OnPuzzleUnsolved()
        {
            nextPuzzleButton.SetActive(false);
        }
        public void OnPuzzleSolved()
        {
            nextPuzzleButton.SetActive(true);
        }

        [Button, DisableInEditorMode]
        public void NextLevel()
        {
            if (currentLevel >= levels.Count)
                return;
            
            nextPuzzleButton.SetActive(false);
            GameStats.Instance.SetCoinsPerSecond(levels[currentLevel].incomeReward);
            currentLevel++;
            
            if(puzzle) puzzle.OnPuzzleSolved -= OnPuzzleSolved;
            if(puzzle) puzzle.OnPuzzleUnsolved -= OnPuzzleUnsolved;
            if(puzzleObj) Destroy(puzzleObj);
            if(currentLevel >= levels.Count) return;

            StartCoroutine(WaitThenSpawnPuzzle());
        }

        private float progress;
        IEnumerator WaitThenSpawnPuzzle()
        {
            progress = 0;
            loadingPuzzleSlider.gameObject.SetActive(true);
            loadingPuzzleSlider.value = 0;
            while (progress < delayBetweenPuzzles)
            {
                progress += Time.deltaTime;
                loadingPuzzleSlider.value = progress / delayBetweenPuzzles;
                yield return null;
            }
            loadingPuzzleSlider.gameObject.SetActive(false);
            SpawnPuzzle(currentLevel);
        }
        
        protected void SpawnPuzzle(int level)
        {
            if (currentLevel >= levels.Count)
                return;
            
            puzzleObj = Instantiate(levels[level].puzzlePrefab, transform);
            puzzleObj.transform.localPosition = Vector3.zero;
            puzzle = puzzleObj.GetComponent<LogicPuzzle>();
            puzzle.OnPuzzleSolved += OnPuzzleSolved;
            puzzle.OnPuzzleUnsolved += OnPuzzleUnsolved;
            LogicManager.Instance.RefreshNodes();
            LogicManager.Instance.UpdateStates();
        }


        [Serializable]
        public class PuzzleLevel
        {
            public int incomeReward;
            public GameObject puzzlePrefab;
        }
    }
}
