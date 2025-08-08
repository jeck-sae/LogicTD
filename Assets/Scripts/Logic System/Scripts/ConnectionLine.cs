using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ConnectionLine : MonoBehaviour
{
    public ConnectionPoint output;
    public ConnectionPoint input;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    public void Initialize(ConnectionPoint from, ConnectionPoint to)
    {
        output = from;
        input = to;
        UpdateLine();
    }

    void Update()
    {
        if (output != null && input != null)
        {
            UpdateLine();
        }
    }

    void UpdateLine()
    {
        lr.SetPosition(0, output.transform.position);
        lr.SetPosition(1, input.transform.position);
    }

    public bool IsMouseNearLine(Vector3 mousePos)
    {
        Vector3 a = output.transform.position;
        Vector3 b = input.transform.position;
        float distance = HandleUtility.DistancePointLine(mousePos, a, b);
        return distance < 0.2f;
    }
}
