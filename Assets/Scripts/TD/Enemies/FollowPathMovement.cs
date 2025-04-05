using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace TowerDefense
{
    public class FollowPathMovement : MonoBehaviour
    {
        [DisableInEditorMode] public Vector3 positionOffset;
    
        public event Action OnArrive;
    
        protected List<Tile> path;
        protected int currentTileIndex;
        protected Vector3 nextTargetPosition;
    
        bool reachedTarget;

        private void Awake()
        {
            UpdatePath();
        }

        public void SetPositionOffset(Vector2 positionOffset)
        {
            this.positionOffset = positionOffset;
        }
    
        protected void UpdatePath()
        {
            if (reachedTarget) return;
            var currentTile = GridManager.Instance.Get(transform.position);
            path = PathManager.Instance.GetPath(currentTile.Position);
            
            if (path == null || path.Count == 0) 
                return;
    
            currentTileIndex = 0;
            nextTargetPosition = path[0].transform.position + positionOffset;
        }
    
        public void SetPath(List<Tile> path)
        {
            this.path = path;
            if(path == null || path.Count == 0)
                return;
    
            reachedTarget = false;
            currentTileIndex = 0;
            nextTargetPosition = path[0].transform.position + positionOffset;
        }
    
            
        public void Move(float amount)
        {
            if (path == null)
            {
                UpdatePath();
    
                if (path == null)
                    return;
            }
    
            if (currentTileIndex >= path.Count)
                return;
    
            float distance = Vector3.Distance(transform.position, nextTargetPosition);
            if (amount >= distance)
            {
                transform.position = nextTargetPosition;
                amount -= distance;
    
                currentTileIndex++;
    
                if (currentTileIndex == path.Count && path[currentTileIndex - 1].IsHome)
                {
                    OnArrive?.Invoke();
                    reachedTarget = true;
                    return;
                }
    
                if (path.Count <= 1 || path[currentTileIndex] == null || !path[currentTileIndex].IsWalkable)
                {
                    UpdatePath();
                    if (path == null)
                        return;
                }
    
                nextTargetPosition = path[currentTileIndex].transform.position + positionOffset;
            }
    
            Vector3 dir = (nextTargetPosition - transform.position).normalized;
            transform.position += dir * amount;
        }
    
    
    
        public float DistanceFromTarget()
        {
            if (path == null) return float.MaxValue;
            return path.Count - currentTileIndex - 1 + Vector3.Distance(transform.position, nextTargetPosition);
        }
    }
}
