using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace TowerDefense
{
    public class ProceduralLevelManager : MonoBehaviour
    {
        public GameObject pathTilePrefab;
        public GameObject expandButtonPrefab;
        
        [ShowInInspector]
        Dictionary<Vector2Int, MapChunk> chunks = new();
        
        public int chunkSize = 5;
        public int branchFrequency;
        public AnimationCurve weightCurve;

        //HashSet<Vector2Int> nearPath = new();
        List<Vector2Int> openEnds = new();
        List<GameObject> buttons = new();

        private void Start()
        {
            SetupStartChunk();
        }

        void SetupStartChunk()
        {
            MapChunk start = new MapChunk();
            start.chunkCoordinates = Vector2Int.zero;
            start.from = Direction.None;
            start.next.Add(Direction.Up, chunkSize/2);
            chunks.Add(Vector2Int.zero, start);
            GenerateChunk(start);
            openEnds.Add(Vector2Int.up);

            foreach (var openEnd in start.next)
                CreateExpandButton(start, openEnd.Key);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                buttons.ForEach(x => x.gameObject.SetActive(true));

                /*GridManager.Instance.Clear();
                var path = ProceduralMapGeneration.GeneratePath(-Vector2Int.one * chunkSize / 2, Vector2Int.one * chunkSize / 2, -Vector2Int.one * chunkSize / 2, Vector2Int.one * chunkSize / 2, weightCurve);
                foreach (var t in path)
                {
                    var tile = Instantiate(pathTilePrefab).GetComponent<Tile>();
                    GridManager.Instance.AddTile(t, tile);
                }*/
            }
        }

        public void ClickedExpandMapButton(InteractableButton button, MapChunk from, Direction towards)
        {
            Destroy(button.gameObject);

            var chunk = CreateChunk(from, towards, branchFrequency);
            GenerateChunk(chunk);

            foreach (var openEnd in chunk.next)
                if (openEnd.Key != Direction.None)
                    CreateExpandButton(chunk, openEnd.Key);
            

            //buttons.ForEach(x => x.gameObject.SetActive(false));
        }

        void CreateExpandButton(MapChunk expandFrom, Direction direction) 
        {
            var button = Instantiate(expandButtonPrefab).GetComponent<InteractableButton>();
            button.Initialize(() => { ClickedExpandMapButton(button, expandFrom, direction); });
            button.transform.localScale = Vector3.one * chunkSize;
            button.transform.position = (expandFrom.chunkCoordinates + DirectionToVector[direction]).ToVector3() * chunkSize + new Vector3Int(chunkSize, chunkSize) / 2;
            buttons.Add(button.gameObject);
        }

        public MapChunk CreateChunk(MapChunk fromChunk, Direction towards, float splitChance)
        {
            Vector2Int chunkCoords = ((fromChunk != null) ? fromChunk.chunkCoordinates : Vector2Int.zero) + DirectionToVector[towards];
            if (chunks.TryGetValue(chunkCoords, out var chunk))
                return chunk;

            chunk = new MapChunk();
            chunks.Add(chunkCoords, chunk);

            Direction from = Opposite[towards];
            chunk.from = from;
            chunk.chunkCoordinates = chunkCoords;
            if (fromChunk != null)
                chunk.fromOffset = fromChunk.next[towards];


            List<Direction> possibleDirections = new() { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

            if (chunks.ContainsKey(chunkCoords + Vector2Int.up) 
              || openEnds.Contains(chunkCoords + Vector2Int.up))
                possibleDirections.Remove(Direction.Up);
            if (chunks.ContainsKey(chunkCoords + Vector2Int.down) 
              || openEnds.Contains(chunkCoords + Vector2Int.down))
                possibleDirections.Remove(Direction.Down);
            if (chunks.ContainsKey(chunkCoords + Vector2Int.left) 
              || openEnds.Contains(chunkCoords + Vector2Int.left))
                possibleDirections.Remove(Direction.Left);
            if (chunks.ContainsKey(chunkCoords + Vector2Int.right) 
              || openEnds.Contains(chunkCoords + Vector2Int.right))
                possibleDirections.Remove(Direction.Right);

            if (from != Direction.None && possibleDirections.Contains(from))
                possibleDirections.Remove(from);

            if (possibleDirections.Count != 0)
            {
                do
                {
                    var i = Random.Range(0, possibleDirections.Count);
                    var offset = Random.Range(1, chunkSize - 1);
                    chunk.next.Add(possibleDirections[i], offset);
                    openEnds.Add(chunkCoords + DirectionToVector[possibleDirections[i]]);
                    possibleDirections.RemoveAt(i);

                } while (possibleDirections.Count > 0 && Random.Range(0, 100) < splitChance);
            }
            else
            {
                chunk.next.Add(Direction.None, 0);
            }
            
            openEnds.Remove(chunk.chunkCoordinates);
            return chunk;
        }


        void GenerateChunk(MapChunk chunkInfo)
        {
            Vector2Int startPos = GetPointOnSide(chunkInfo.chunkCoordinates, chunkInfo.from, chunkInfo.fromOffset);
            Vector2Int chunkCoords = chunkInfo.chunkCoordinates * chunkSize;

            HashSet<Vector2Int> path = new();

            foreach (var dir in chunkInfo.next)
            {
                Vector2Int endPos = GetPointOnSide(chunkInfo.chunkCoordinates, dir.Key, dir.Value);
                path.AddRange(ProceduralMapGeneration.GeneratePath(
                    chunkCoords, chunkCoords + Vector2Int.one * (chunkSize - 1), 
                    startPos, endPos, weightCurve, path));
            }

            foreach (var t in path)
            {
                var tile = Instantiate(pathTilePrefab).GetComponent<Tile>();
                GridManager.Instance.AddTile(t, tile);
            }

        }



        public Vector2Int GetPointOnSide(Vector2Int chunkCoords, Direction direction, int offset)
        {
            Vector2Int chunkZero = chunkCoords * chunkSize;
            switch (direction)
            {
                case Direction.Up:
                    return chunkZero + new Vector2Int(offset, chunkSize - 1);
                case Direction.Down:
                    return chunkZero + new Vector2Int(offset, 0);
                case Direction.Left:
                    return chunkZero + new Vector2Int(0, offset);
                case Direction.Right:
                    return chunkZero + new Vector2Int(chunkSize - 1, offset);
                default:
                    return chunkZero + Vector2Int.one * chunkSize / 2;
            }

        }


        private void OnDrawGizmos()
        {
            foreach(var chunk in chunks)
            {
                Gizmos.DrawWireCube((chunk.Key * chunkSize + Vector2Int.one * chunkSize / 2).ToVector3(), Vector3.one * chunkSize);
            }
        }

        public readonly Dictionary<Direction, Vector2Int> DirectionToVector = new()
        {
            { Direction.Up,     Vector2Int.up    },
            { Direction.Down,   Vector2Int.down  },
            { Direction.Left,   Vector2Int.left  },
            { Direction.Right,  Vector2Int.right },
            { Direction.None,   Vector2Int.zero  },
        };
        public readonly Dictionary<Vector2Int, Direction> VectorToDirection = new()
        {
            { Vector2Int.up,    Direction.Up     },
            { Vector2Int.down,  Direction.Down   },
            { Vector2Int.left,  Direction.Left   },
            { Vector2Int.right, Direction.Right  },
            { Vector2Int.zero,  Direction.None   },
        };
        public readonly Dictionary<Direction, Direction> Opposite = new()
        {
            { Direction.Left, Direction.Right },
            { Direction.Right, Direction.Left },
            { Direction.Up, Direction.Down },
            { Direction.Down, Direction.Up},
            { Direction.None, Direction.None },
        };

        public class MapChunk
        {
            public Vector2Int chunkCoordinates;
            public Direction from;
            public int fromOffset;
            public Dictionary<Direction, int> next = new();

        }


        public enum Direction { None, Up, Down, Left, Right }
    }
}
