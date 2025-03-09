using System.IO;
using UnityEngine;


namespace TowerDefense
{
    public class TryLoadSelectedLevel : MonoBehaviour
    {
        [SerializeField] string debugLevelIfNoSelected;
    
        private void Start()
        {
            if (LevelSelect.SelectedLevel != null)
            {
                GridImportExport.LoadGrid(GridManager.Instance, LevelSelect.SelectedLevel);
            }
            else if (!string.IsNullOrEmpty(debugLevelIfNoSelected))
            {
                string level = Path.Combine(Application.streamingAssetsPath, GridImportExport.LEVELS_PATH, debugLevelIfNoSelected + ".td");
                GridImportExport.ImportAndLoadGrid(GridManager.Instance, level);
            }
            else if (!string.IsNullOrEmpty(SaveManager.Instance.data.lastSelectedLevel))
            {
                string level = Path.Combine(Application.streamingAssetsPath, GridImportExport.LEVELS_PATH, SaveManager.Instance.data.lastSelectedLevel + ".td");
                GridImportExport.ImportAndLoadGrid(GridManager.Instance, level);
            }
        }
    }
    
}
