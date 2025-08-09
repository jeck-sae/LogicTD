using UnityEngine;

public class ClickablePoint : MonoBehaviour
{
    public LogicGate parentGate;
    public bool isOutput; // Set in inspector

    private ConnectionManager connectionManager;

    void Start()
    {
        connectionManager = FindAnyObjectByType<ConnectionManager>();
    }

    void OnMouseDown()
    {
        if (connectionManager != null)
        {
            connectionManager.OnClickConnectionPoint(parentGate, transform, isOutput);
        }
    }
}
