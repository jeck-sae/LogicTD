using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TowerDefense
{
    public class GridManager : Singleton<GridManager>
    {
        [ShowInInspector] Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
        public Action<Tile> OnTileAdded;
        public Action<Vector2Int> OnTileRemoved;

        public int Count => tiles.Count;
        
        public void AddTile(Vector2Int position, Tile tile)
        {
            if (tiles.ContainsKey(position))
            {
                Debug.Log("the slot " + position + " is already occupied");
                return;
            }
    
            tiles[position] = tile;
            tile.InitializeTile(position);
    
            GetAdjacentTiles(position).ForEach(x=>x.OnNearbyTileChanged());
            OnTileAdded?.Invoke(tile);
        }
    
    
        public List<Tile> GetAdjacentTiles(Vector2Int position)
        {
            List<Tile> neighbors = new List<Tile>();
    
            if (tiles.TryGetValue(position + Vector2Int.right, out Tile right))
                neighbors.Add(right);
            if (tiles.TryGetValue(position + Vector2Int.left, out Tile left))
                neighbors.Add(left);
            if (tiles.TryGetValue(position + Vector2Int.up, out Tile up))
                neighbors.Add(up);
            if (tiles.TryGetValue(position + Vector2Int.down, out Tile down))
                neighbors.Add(down);
    
            return neighbors;
        }
    
        public List<Tile> GetHomes()
        {
            return tiles.Where(x => x.Value.IsHome).Select(x => x.Value).ToList();
        }
    
    
        public static Vector2Int FixCoordinates(Vector2 position) => new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        public static Vector2Int FixCoordinates(Vector3 position) => new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        public Tile Get(Vector3 position) => Get(FixCoordinates(position));
        public Tile Get(Vector2Int position)
        {
            if (tiles.ContainsKey(position))
                return tiles[position];
            return null;
        }
    
        public IEnumerable<Tile> GetAll() => tiles.Values;
    
        public void Remove(Tile tile)
            => Remove(tiles.FirstOrDefault(x => x.Value == tile).Key);
        public void Remove(Vector2Int position)
        {
            tiles.Remove(position);
            GetAdjacentTiles(position).ForEach(x=>x.OnNearbyTileChanged());
            OnTileRemoved?.Invoke(position);
        }
    
        public void Clear()
        {
            while (tiles.Count > 0)
            {
                var t = tiles.First().Value;
                Remove(t);
                Destroy(t.gameObject);
            }
        }
    
        public bool Contains(Vector2Int position) => tiles.ContainsKey(position);
        public bool Contains(Tile tile) => tiles.ContainsValue(tile);
    
        Tile cachedHoveringTile;
        float cachedHoveringTileTime;
        public Tile GetHoveringTile()
        {
            if (Time.time == cachedHoveringTileTime)
                return cachedHoveringTile;
    
            var mousePos = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition);
            cachedHoveringTile = Get(mousePos);
            cachedHoveringTileTime = Time.time;
            return cachedHoveringTile;
        }
    }
}
