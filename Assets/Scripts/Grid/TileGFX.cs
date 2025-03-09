using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;


namespace TowerDefense
{
    public class TileGFX : MonoBehaviour
    {
        [SerializeField, FoldoutGroup("Renderers")] GameObject leftBorder;
        [SerializeField, FoldoutGroup("Renderers")] GameObject rightBorder;
        [SerializeField, FoldoutGroup("Renderers")] GameObject upBorder;
        [SerializeField, FoldoutGroup("Renderers")] GameObject downBorder;
    
        [SerializeField, FoldoutGroup("Renderers")] GameObject upShadow;
        [SerializeField, FoldoutGroup("Renderers")] GameObject leftShadow;
        [SerializeField, FoldoutGroup("Renderers")] GameObject downShadow;
        [SerializeField, FoldoutGroup("Renderers")] GameObject rightShadow;
    
        [SerializeField, FoldoutGroup("Renderers")] GameObject upRightShadow;
        [SerializeField, FoldoutGroup("Renderers")] GameObject upLeftShadow;
        [SerializeField, FoldoutGroup("Renderers")] GameObject downRightShadow;
        [SerializeField, FoldoutGroup("Renderers")] GameObject downLeftShadow;
    
        Tile tile;
    
        private void Awake()
        {
            tile = GetComponent<Tile>();
        }
    
        private void Start()
        {
            UpdateGFX();
        }
    
        public void UpdateGFX()
        {
            var neighbours = GridManager.Instance.GetAdjacentTiles(tile.Position);
    
            leftBorder.SetActive(true);
            rightBorder.SetActive(true);
            upBorder.SetActive(true);
            downBorder.SetActive(true);
    
            foreach (var neighbor in neighbours)
            {
                if (!neighbor.Type.Equals(tile.Type))
                    continue;
    
                var diff = neighbor.Position - tile.Position;
    
                if (diff == Vector2Int.left)
                    leftBorder.SetActive(false);
                if (diff == Vector2Int.right)
                    rightBorder.SetActive(false);
                if (diff == Vector2Int.up)
                    upBorder.SetActive(false);
                if (diff == Vector2Int.down)
                    downBorder.SetActive(false);
            }
    
            //enable shadow sides
            leftShadow.SetActive(!GridManager.Instance.Contains(tile.Position + Vector2Int.left));
            rightShadow.SetActive(!GridManager.Instance.Contains(tile.Position + Vector2Int.right));
            downShadow.SetActive(!GridManager.Instance.Contains(tile.Position + Vector2Int.down));
            upShadow.SetActive(!GridManager.Instance.Contains(tile.Position + Vector2Int.up));
    
            //enable shadow corners
            upLeftShadow.SetActive(
                !GridManager.Instance.Contains(tile.Position + Vector2Int.up + Vector2Int.left)
                && !GridManager.Instance.Contains(tile.Position + Vector2Int.up)
                && !GridManager.Instance.Contains(tile.Position + Vector2Int.left));
    
            upRightShadow.SetActive(
                !GridManager.Instance.Contains(tile.Position + Vector2Int.up + Vector2Int.right)
                && !GridManager.Instance.Contains(tile.Position + Vector2Int.up)
                && !GridManager.Instance.Contains(tile.Position + Vector2Int.right));
    
            downLeftShadow.SetActive(
                !GridManager.Instance.Contains(tile.Position + Vector2Int.down + Vector2Int.left)
                && !GridManager.Instance.Contains(tile.Position + Vector2Int.down)
                && !GridManager.Instance.Contains(tile.Position + Vector2Int.left));
    
            downRightShadow.SetActive(
                !GridManager.Instance.Contains(tile.Position + Vector2Int.down + Vector2Int.right)
                && !GridManager.Instance.Contains(tile.Position + Vector2Int.down)
                && !GridManager.Instance.Contains(tile.Position + Vector2Int.right));
    
    
    
        }
    }
    
}
