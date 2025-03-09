using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace TowerDefense
{
    public class LevelSelect : MonoBehaviour
    {
        [SerializeField] List<GridImportExport.LevelInfo> levels;
        public string levelsPath;
        int currentLevelIndex = 0;
        public static GridImportExport.LevelInfo SelectedLevel;
    
        private void Awake()
        {
            ImportLevels();
        }
    
        void Start()
        {
            var l = levels.FirstOrDefault(x => x.name == SaveManager.Instance.data.lastSelectedLevel);
            currentLevelIndex = (l != null) ? levels.IndexOf(l) : 0;
            SelectLevel(levels[currentLevelIndex]);
        }
    
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentLevelIndex--;
                if (currentLevelIndex < 0)
                    currentLevelIndex = levels.Count - 1;
                SelectLevel(levels[currentLevelIndex]);
            }
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentLevelIndex++;
                if (currentLevelIndex >= levels.Count)
                    currentLevelIndex = 0;
                SelectLevel(levels[currentLevelIndex]);
            }
        }
    
        public void ImportLevels()
        {
            levels = new();
            string path = Path.Combine(Application.streamingAssetsPath, levelsPath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
    
            foreach (var f in Directory.GetFiles(path))
            {
                if (f.EndsWith(".td"))
                {
                    var level = GridImportExport.ImportGrid(f);
                    levels.Add(level);
                }
            }
        }
    
    
        public void SelectLevel(GridImportExport.LevelInfo level)
        {
            GridImportExport.LoadGrid(GridManager.Instance, level);
            SelectedLevel = level;
    
            SaveManager.Instance.data.lastSelectedLevel = level.name;
            SaveManager.SaveState();
        }
    }
    
}
