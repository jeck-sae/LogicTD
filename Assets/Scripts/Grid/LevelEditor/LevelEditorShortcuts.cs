using UnityEngine;
using System.Collections.Generic;
using SFB;

public class LevelEditorShortcuts : MonoBehaviour
{
    [SerializeField] List<GameObject> tiles;


    void Update()
    {
        // 1, 2... > Select tiles
        if (Input.GetKeyDown(KeyCode.Alpha1) && tiles.Count > 0)
            LevelEditor.Instance.SetPlacingTile(tiles[0]);
        if(Input.GetKeyDown(KeyCode.Alpha2) && tiles.Count > 1)
            LevelEditor.Instance.SetPlacingTile(tiles[1]);
        if(Input.GetKeyDown(KeyCode.Alpha3) && tiles.Count > 2)
            LevelEditor.Instance.SetPlacingTile(tiles[2]);
        if(Input.GetKeyDown(KeyCode.Alpha4) && tiles.Count > 3)
            LevelEditor.Instance.SetPlacingTile(tiles[3]);
        if(Input.GetKeyDown(KeyCode.Alpha5) && tiles.Count > 4)
            LevelEditor.Instance.SetPlacingTile(tiles[4]);
        if(Input.GetKeyDown(KeyCode.Alpha6) && tiles.Count > 5)
            LevelEditor.Instance.SetPlacingTile(tiles[5]);
        if(Input.GetKeyDown(KeyCode.Alpha7) && tiles.Count > 6)
            LevelEditor.Instance.SetPlacingTile(tiles[6]);
        if(Input.GetKeyDown(KeyCode.Alpha8) && tiles.Count > 7)
            LevelEditor.Instance.SetPlacingTile(tiles[7]);

        //Ctrl+S > Save
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
        {
            var path = StandaloneFileBrowser.SaveFilePanel("Save Level", Application.streamingAssetsPath, "level", "td");

            if (path.Length != 0)
            {
                LevelEditor.Instance?.EnablePreview(false);
                GridImportExport.ExportGrid(GridManager.Instance, path);
                LevelEditor.Instance?.EnablePreview(true);
            }
        }
        
        //Ctrl+L > Load level
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Load Level", "", "td", false);

            if(paths.Length <= 0) return;
            string path = paths[0];

            if (path.Length != 0)
                GridImportExport.ImportAndLoadGrid(GridManager.Instance, path);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
        {
            //Ctrl+Shift+F > Fix broken tiles
            if (Input.GetKeyDown(KeyCode.F))
            {
                Tile[] tiles = FindObjectsByType(typeof(Tile), FindObjectsSortMode.None) as Tile[];
                foreach (var t in tiles)
                {
                    if (!GridManager.Instance.Contains(t) && LevelEditor.Instance.lastHoveredTile != t)
                        Destroy(t.gameObject);
                }
            }

            //Ctrl+Shift+C+A > Clear grid
            if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.A))
            {
                GridManager.Instance.Clear();
                //GridManager.Instance.GetAll().Clear();
                //Tile[] tiles = FindObjectsByType(typeof(Tile), FindObjectsSortMode.None) as Tile[];
                //tiles.ForEach((t) => { if (t != LevelEditor.Instance.placingTile) Destroy(t.gameObject); });
            }

        }
    }


}
