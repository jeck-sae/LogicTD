using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Sirenix.OdinInspector;


namespace TowerDefense
{
    public class PathManager : Singleton<PathManager>
    {
        public float cacheDuration = 5;
    
        [ShowInInspector, ReadOnly]
        public Dictionary<Vector2Int, SavedPath> savedPaths = new();
    
        public List<Tile> GetPath(Vector2Int startPosition)
        {
            SavedPath path;
            
            if (savedPaths.TryGetValue(startPosition, out path))
                return path.path;
    
            path = new SavedPath();
    
            path.path = Pathfinder.FindNearestTile(GridManager.Instance, 
                GridManager.Instance.Get(startPosition));
    
            path.destroyPathAction = () => { //unsubscribe from all the events once the path is removed
                path.path.ForEach(t => { t.OnTileChanged -= path.destroyPathAction; });
                savedPaths.Remove(startPosition);
            };
            
            savedPaths.Add(startPosition, path);
    
            if (path != null) //instantly delete the path if one of its tiles is removed
                path.path.ForEach(t => { t.OnTileChanged += path.destroyPathAction; });
    
            StartCoroutine(DeleteAfterSeconds(startPosition, path, cacheDuration));
            return path.path;
        }
        IEnumerator DeleteAfterSeconds(Vector2Int startPosition, SavedPath path, float seconds)
        {
            yield return Helpers.GetWait(seconds);
    
            if (!savedPaths.ContainsKey(startPosition) && savedPaths[startPosition] == path)
                savedPaths[startPosition].destroyPathAction();
        }
    
        public class SavedPath
        {
            public List<Tile> path;
            public Action destroyPathAction;
        }
    }
    
}
