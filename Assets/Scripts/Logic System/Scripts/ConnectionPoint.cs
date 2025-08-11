using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ConnectionPoint : MonoBehaviour
{
    public bool isOutput;                 // true if output, false if input
    public bool isInput => !isOutput;

    [HideInInspector] public ConnectionLine connectedLine;

    public Vector3 Position => transform.position;

    private void OnMouseDown()
    {
        Debug.Log($"Clicked {gameObject.name} (isOutput={isOutput})");
        if (ConnectionManager.Instance != null)
        {
            ConnectionManager.Instance.PointClicked(this);
        }
    }
}
