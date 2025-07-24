using UnityEngine;

public class ClickablePoint : MonoBehaviour
{
    public LogicGate parentGate;
    public bool isOutput; // true = output point, false = input point

    private ConnectionManager connectionManager;

    void Start()
    {
        connectionManager = FindAnyObjectByType<ConnectionManager>();
    }

    void OnMouseDown()
    {
        if (connectionManager != null)
        {
            if (isOutput)
            {
                connectionManager.StartConnectionFromPoint(parentGate, transform);
            }
            else
            {
                connectionManager.EndConnectionAtPoint(parentGate, transform);
            }
        }
    }
}
