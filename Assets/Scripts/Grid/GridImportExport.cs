using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;


namespace TowerDefense
{
    public class GridImportExport
    {
        public const string TILES_PATH = "Prefabs/Tiles/";
        public const string LEVELS_PATH = "Levels/";
    
        private static Dictionary<string, Tile> tilePrefabs;
    
        public static void ExportGrid(GridManager grid, string saveLocation)
        {
            var tiles = grid.GetAll().ToArray();
            
            var levelInfo = new LevelInfo(tiles);
            string gridData = JsonUtility.ToJson(levelInfo);
            File.WriteAllText(saveLocation, gridData);
        }
    
        public static void ImportAndLoadGrid(GridManager grid, string loadLocation)
        {
            var levelInfo = ImportGrid(loadLocation);
            LoadGrid(grid, levelInfo);
        }
    
        public static LevelInfo ImportGrid(string loadLocation)
        {
            string gridData = File.ReadAllText(loadLocation);
            var levelInfo = JsonUtility.FromJson<LevelInfo>(gridData);
    
            var splitFileName = loadLocation.Split('/', '\\').Last().Split('.');
            levelInfo.name = splitFileName[splitFileName.Length - 2];
    
            return levelInfo;
        }
    
        public static void LoadGrid(GridManager grid, LevelInfo levelInfo)
        {
            if(tilePrefabs == null)
                LoadTilePrefabs();
    
            grid.Clear();
            foreach (var t in levelInfo.tiles)
            {
                var go = GameObject.Instantiate(tilePrefabs[t.tileId].gameObject);
                var tile = go.GetComponent<Tile>();
                grid.AddTile(t.position, tile);
            }
        }
    
        private static void LoadTilePrefabs()
        {
            tilePrefabs = new();
            var prefabs = Resources.LoadAll(TILES_PATH);
            foreach(GameObject go in prefabs)
            {
                Tile t = go.GetComponent<Tile>();
                tilePrefabs.Add(t.tileId, t);
            }
        }
    
        [System.Serializable]
        public class LevelInfo
        {
            [DoNotSerialize]
            public string name;
            public List<SerializedTile> tiles;
            public LevelInfo(Tile[] tiles)
            {
                var serializedTiles = new List<SerializedTile>();

                int minX = int.MaxValue;
                int minY = int.MaxValue;
                int maxX = int.MinValue;
                int maxY = int.MinValue;

                foreach (var tile in tiles)
                {
                    if (tile.Position.x < minX) minX = tile.Position.x;
                    if (tile.Position.y < minY) minY = tile.Position.y;
                    if (tile.Position.x > maxX) maxX = tile.Position.x;
                    if (tile.Position.y > maxY) maxY = tile.Position.y;
                    serializedTiles.Add(new SerializedTile(tile));
                }
                
                int width = maxX - minX;
                int height = maxY - minY;

                int offsetX = - (width / 2) - minX;
                int offsetY = - (height / 2) - minY;

                Vector2Int offset = new Vector2Int(offsetX, offsetY);

                foreach(var tile in serializedTiles)
                    tile.position += offset;

                this.tiles = serializedTiles;
            }
            
            
            [System.Serializable]
            public class SerializedTile
            {
                public string tileId;
                public Vector2Int position;
                public SerializedTile(Tile tile)
                {
                    tileId = tile.tileId;
                    position = tile.Position;
                }
            }
        }
    }
    
}
