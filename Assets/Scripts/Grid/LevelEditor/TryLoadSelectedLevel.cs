using UnityEngine;

public class TryLoadSelectedLevel : MonoBehaviour
{
    private void Start()
    {
        if(LevelSelect.SelectedLevel != null)
        {
            GridImportExport.LoadGrid(GridManager.Instance, LevelSelect.SelectedLevel);
        }
    }
}
