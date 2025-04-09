using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TowerDefense
{
    public class Pathfinder
    {

        public static List<Tile> FindNearestTile(GridManager grid, Tile startTile, Func<Tile, bool> isDestination = null)
        {
            Dictionary<Tile, PathfinderTile> tileData = new Dictionary<Tile, PathfinderTile>();
            Heap<PathfinderTile> openSet = new Heap<PathfinderTile>(GridManager.Instance.Count);
            HashSet<Tile> closedSet = new HashSet<Tile>();
    
            if (isDestination == null)
                isDestination = (t) => { return t.IsHome; };
    
            tileData.Add(startTile, new PathfinderTile(startTile, 0, int.MaxValue));
            openSet.Add(tileData[startTile]);
    
            while (openSet.Count > 0)
            {
                PathfinderTile currentTile = openSet.RemoveFirst();

                if (isDestination(currentTile.originalTile))
                    return RetracePath(tileData[startTile], currentTile);
    
                closedSet.Add(currentTile.originalTile);
    
                foreach (Tile neighbour in grid.GetAdjacentTiles(currentTile.originalTile.Position))
                {
                    if (!neighbour || !neighbour.IsWalkable || closedSet.Contains(neighbour))
                        continue;
    
                    int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile.originalTile, neighbour);
    
                    if (!tileData.ContainsKey(neighbour))
                    {
                        tileData.Add(neighbour, new PathfinderTile(neighbour, newMovementCostToNeighbour, currentTile.hCost - 1, currentTile));
                        openSet.Add(tileData[neighbour]);
                    }
                }
            }
    
            List<Tile> noPath = new List<Tile>();
            noPath.Add(startTile);
            return noPath;
        }
    
        public static List<Tile> FindPath(GridManager grid, Tile startTile, Tile endTile)
        {

            Dictionary<Tile, PathfinderTile> tileData = new Dictionary<Tile, PathfinderTile>();
            Heap<PathfinderTile> openSet = new Heap<PathfinderTile>(GridManager.Instance.Count);
            HashSet<Tile> closedSet = new HashSet<Tile>();

            tileData.Add(startTile, new PathfinderTile(startTile, 0, GetDistance(startTile, endTile), null));
            openSet.Add(tileData[startTile]);

            while (openSet.Count > 0)
            {
                PathfinderTile currentTile = openSet.RemoveFirst();
                currentTile.open = false;
                closedSet.Add(currentTile.originalTile);

                if (currentTile.originalTile == endTile)
                    return RetracePath(tileData[startTile], currentTile);
    
    
                foreach (Tile neighbour in grid.GetAdjacentTiles(currentTile.originalTile.Position))
                {
                    if (!neighbour || !neighbour.IsWalkable || closedSet.Contains(neighbour))
                        continue;
                    int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile.originalTile, neighbour);

                    if (!tileData.ContainsKey(neighbour))
                    {
                        tileData.Add(neighbour, new PathfinderTile(neighbour, newMovementCostToNeighbour, GetDistance(neighbour, endTile), currentTile));
                        openSet.Add(tileData[neighbour]);
                    }
                    
                    if (newMovementCostToNeighbour < tileData[neighbour].gCost)
                    {
                        tileData[neighbour].gCost = newMovementCostToNeighbour;
                        tileData[neighbour].parent = currentTile;
                    }
                }
            }
    
            List<Tile> noPath = new List<Tile>();
            noPath.Add(startTile);
            return noPath;
        }
    
        static List<Tile> RetracePath(PathfinderTile startTile, PathfinderTile endTile)
        {
            List<Tile> path = new List<Tile>();
            PathfinderTile currentTile = endTile;
    
            while (currentTile != startTile)
            {
                path.Add(currentTile.originalTile);
                currentTile = currentTile.parent;
            }
            //path.Add(startTile.originalTile);
            path.Reverse();
            return path;
        }
    
        public static int GetDistance(Tile t1, Tile t2) => GetDistance(t1.Position, t2.Position);
        public static int GetDistance(Vector2Int t1, Vector2Int t2)
        {
            return Mathf.RoundToInt(Mathf.Abs(t1.x - t2.x) + Mathf.Abs(t1.y - t2.y));
        }

        class PathfinderTile : IHeapItem<PathfinderTile>
        {
            public int HeapIndex { get; set; }
            public bool walkable => originalTile != null && originalTile.IsWalkable;
            public int gCost;
            public int hCost;
            public int fCost { get { return gCost + hCost; } }

            public bool open = true;

            public Tile originalTile;
            public PathfinderTile parent;
    
    
            public PathfinderTile(Tile _tile, int _gCost, int _hCost, PathfinderTile _parent = null)
            {
                originalTile = _tile;
                gCost = _gCost;
                hCost = _gCost;
                parent = _parent;
            }
    
            public int CompareTo(PathfinderTile other)
            {
                int compare = fCost.CompareTo(other.fCost);
                if (compare == 0)
                {
                    compare = hCost.CompareTo(other.hCost);
                }
                return -compare;
            }
        }
    }
}
