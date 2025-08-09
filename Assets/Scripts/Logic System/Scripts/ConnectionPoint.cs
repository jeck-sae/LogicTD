using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ConnectionPoint : MonoBehaviour
{
    public bool isOutput; // True if output point
    public bool isInput => !isOutput; // Auto property for input

    public Vector3 Position => transform.position;

    private void OnMouseDown()
    {
        if (ConnectionManager.Instance != null)
        {
            ConnectionManager.Instance.PointClicked(this);
        }
    }
}
