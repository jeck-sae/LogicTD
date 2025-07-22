using UnityEngine;

public class GateClickable : MonoBehaviour
{
    public LogicGate logicGate;
    private ConnectionManager connectionManager;


    void Start()
    {
        connectionManager = FindAnyObjectByType<ConnectionManager>();
        logicGate = GetComponentInParent<LogicGate>();
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked: " + logicGate.name);
        if (connectionManager != null)
        {
            if (connectionManager.fromGate == null)
                connectionManager.StartConnection(logicGate);
            else
                connectionManager.EndConnection(logicGate);
        }
    }
}
