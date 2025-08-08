using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    public bool isOutput;
    public LogicGate parentGate;

    private void Start()
    {
        parentGate = GetComponentInParent<LogicGate>();
    }

    private void OnMouseDown()
    {
        if (ConnectionManager.Instance != null)
        {
            ConnectionManager.Instance.HandleClick(this);
        }
    }
}
