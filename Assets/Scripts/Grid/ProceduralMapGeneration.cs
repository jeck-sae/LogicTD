using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense
{
    public class ProceduralMapGeneration : MonoBehaviour
    {
        
        public static List<Vector2Int> GeneratePath(Vector2Int minCoords, Vector2Int maxCoords, Vector2Int startTile, Vector2Int endTile, AnimationCurve weightCurve, HashSet<Vector2Int> existingPath)
        {
            Dictionary<Vector2Int, PathGenerationTile> tileData = new Dictionary<Vector2Int, PathGenerationTile>();
            Heap<PathGenerationTile> openSet = new Heap<PathGenerationTile>((maxCoords.x - minCoords.x) * (maxCoords.y - minCoords.y));
            HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

            tileData.Add(startTile, new PathGenerationTile(startTile, 0, 0, GetDistance(startTile, endTile)));
            openSet.Add(tileData[startTile]);

            while (openSet.Count > 0)
            {
                PathGenerationTile currentTile = openSet.RemoveFirst();

                closedSet.Add(currentTile.position);

                if (currentTile.position == endTile)
                    return RetracePath(tileData[startTile], tileData[endTile]).Select(x => x.position).ToList();


                foreach (Vector2Int neighbour in GetNeighbours(currentTile.position))
                {
                    if (closedSet.Contains(neighbour))
                        continue;

                    int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile.position, neighbour);

                    if (!tileData.ContainsKey(neighbour))
                    {
                        int cost = 0;
                        if (!existingPath.Contains(neighbour))
                        {
                            cost += (int)weightCurve.Evaluate(UnityEngine.Random.Range(0f, 1f));
                            if (neighbour.x == maxCoords.x || neighbour.y == maxCoords.y || neighbour.x == minCoords.x || neighbour.y == minCoords.y)
                                cost += 80;
                        }

                        tileData.Add(neighbour, new PathGenerationTile(neighbour, cost, newMovementCostToNeighbour + cost, GetDistance(neighbour, endTile), currentTile));

                        openSet.Add(tileData[neighbour]);
                    }

                    newMovementCostToNeighbour += tileData[neighbour].cost;
                    

                    if (newMovementCostToNeighbour < tileData[neighbour].gCost)
                    {
                        tileData[neighbour].gCost = newMovementCostToNeighbour;
                        tileData[neighbour].parent = currentTile;
                    }
                }
            }

            List<Vector2Int> noPath = new List<Vector2Int>();
            noPath.Add(startTile);
            return noPath;

            List<Vector2Int> GetNeighbours(Vector2Int centerPos)
            {
                var n = new List<Vector2Int>();
                Vector2Int newPos = centerPos + Vector2Int.up;
                if (newPos.y <= maxCoords.y)
                    n.Add(newPos);
                newPos = centerPos + Vector2Int.down;
                if (newPos.y >= minCoords.y)
                    n.Add(newPos);
                newPos = centerPos + Vector2Int.right;
                if (newPos.x <= maxCoords.x)
                    n.Add(newPos);
                newPos = centerPos + Vector2Int.left;
                if (newPos.x >= minCoords.x)
                    n.Add(newPos);
                return n;
            }
        }

        static List<PathGenerationTile> RetracePath(PathGenerationTile startTile, PathGenerationTile endTile)
        {
            List<PathGenerationTile> path = new List<PathGenerationTile>();
            PathGenerationTile currentTile = endTile;

            while (currentTile != startTile)
            {
                path.Add(currentTile);
                currentTile = currentTile.parent;
            }
            path.Add(startTile);
            path.Reverse();
            return path;
        }

        public static int GetDistance(Vector2Int t1, Vector2Int t2)
        {
            return Mathf.RoundToInt(Mathf.Abs(t1.x - t2.x) + Mathf.Abs(t1.y - t2.y));
        }

        class PathGenerationTile : IHeapItem<PathGenerationTile>
        {
            public int gCost;
            public int hCost;
            public int fCost { get { return gCost + hCost; } }

            public Vector2Int position;
            public int cost;

            public PathGenerationTile parent;


            public PathGenerationTile(Vector2Int pos, int cost, int _gCost, int _hCost, PathGenerationTile _parent = null)
            {
                position = pos; 
                this.cost = cost;
                gCost = _gCost;
                hCost = _gCost;
                parent = _parent;
            }

            public int HeapIndex { get; set; }
            public int CompareTo(PathGenerationTile other)
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
