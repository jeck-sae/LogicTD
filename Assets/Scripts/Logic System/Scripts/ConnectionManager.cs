using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public LogicGate fromGate;                // The gate we start dragging from
    private LineRenderer currentLine;         // The line we draw

    void Update()
    {
        if (currentLine != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;  // keep on 2D plane
            currentLine.SetPosition(1, mousePos);
        }
    }

    public void StartConnection(LogicGate gate)
    {
        fromGate = gate;

        GameObject lineObj = new GameObject("Wire");
        currentLine = lineObj.AddComponent<LineRenderer>();
        currentLine.material = new Material(Shader.Find("Sprites/Default"));
        currentLine.startColor = Color.yellow;
        currentLine.endColor = Color.yellow;
        currentLine.startWidth = 0.05f;
        currentLine.endWidth = 0.05f;
        currentLine.positionCount = 2;

        // Use first output point from the gate
        Vector3 start = gate.GetComponent<GateConnector>().outputPoints[0].position;
        start.z = 0;
        currentLine.SetPosition(0, start);
        currentLine.SetPosition(1, start); // initial position
    }

    public void EndConnection(LogicGate toGate)
    {
        if (fromGate != null && toGate != null && fromGate != toGate)
        {
            // Connect logically
            toGate.AddInput(fromGate);

            // Use first input point from the toGate
            Vector3 end = toGate.GetComponent<GateConnector>().inputPoints[0].position;
            end.z = 0;
            currentLine.SetPosition(1, end);
        }
        else
        {
            // Cancel, destroy line
            Destroy(currentLine.gameObject);
        }

        // Reset for next connection
        fromGate = null;
        currentLine = null;
    }
}
