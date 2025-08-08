using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClickablePoint : MonoBehaviour
{
    public bool isOutput;

    private void OnMouseDown()
    {
        ConnectionManager.Instance.PointClicked(this);
    }
}
