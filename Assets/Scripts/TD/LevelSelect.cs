using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] List<GridImportExport.LevelInfo> levels = new ();
    public string levelsPath;
    int currentLevelIndex = 0;

    public static GridImportExport.LevelInfo SelectedLevel;

    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, levelsPath);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        foreach (var f in Directory.GetFiles(path))
        {
            if (f.EndsWith(".td"))
            {
                var level = GridImportExport.ImportGrid(f);
                levels.Add(level);
                if(levels.Count > 0)
                    SelectLevel(levels[0]);
            }
        }
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


    public void SelectLevel(GridImportExport.LevelInfo level)
    {
        GridImportExport.LoadGrid(GridManager.Instance, level);
        SelectedLevel = level;
    }
}
