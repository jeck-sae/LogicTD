using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

//connecting logic doesn't need inheritance anymore, could make this as a separate MonoBehaviour
public class ConnectingTile : Tile
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

    private void Start()
    {
        UpdateBorders();
    }
    public override void OnNearbyTileChanged()
    {
        UpdateBorders();
    }

    protected void UpdateBorders()
    {
        var neighbours = GridManager.Instance.GetAdjacentTiles(position);

        leftBorder.SetActive(true);
        rightBorder.SetActive(true);
        upBorder.SetActive(true);
        downBorder.SetActive(true);

        foreach (var neighbor in neighbours)
        {
            if (!neighbor.type.Equals(type))
                continue;

            var diff = neighbor.position - position;

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
        leftShadow.SetActive(!GridManager.Instance.Contains(position + Vector2Int.left));
        rightShadow.SetActive(!GridManager.Instance.Contains(position + Vector2Int.right));
        downShadow.SetActive(!GridManager.Instance.Contains(position + Vector2Int.down));
        upShadow.SetActive(!GridManager.Instance.Contains(position + Vector2Int.up));
            
        //enable shadow corners
        upLeftShadow.SetActive(
            !GridManager.Instance.Contains(position + Vector2Int.up + Vector2Int.left)
            && !GridManager.Instance.Contains(position + Vector2Int.up)
            && !GridManager.Instance.Contains(position + Vector2Int.left));

        upRightShadow.SetActive(
            !GridManager.Instance.Contains(position + Vector2Int.up + Vector2Int.right)
            && !GridManager.Instance.Contains(position + Vector2Int.up)
            && !GridManager.Instance.Contains(position + Vector2Int.right));

        downLeftShadow.SetActive(
            !GridManager.Instance.Contains(position + Vector2Int.down + Vector2Int.left)
            && !GridManager.Instance.Contains(position + Vector2Int.down)
            && !GridManager.Instance.Contains(position + Vector2Int.left));

        downRightShadow.SetActive(
            !GridManager.Instance.Contains(position + Vector2Int.down + Vector2Int.right)
            && !GridManager.Instance.Contains(position + Vector2Int.down)
            && !GridManager.Instance.Contains(position + Vector2Int.right));



    }

}
