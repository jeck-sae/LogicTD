using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public LogicGate fromGate;               // Gate where drag started
    private LineRenderer currentLine;        // Line being drawn

    void Update()
    {
        if (currentLine != null)
        {
            // Follow mouse with the end of the line
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Keep on 2D plane
            currentLine.SetPosition(1, mousePos);
        }
    }

    public void StartConnection(LogicGate gate)
    {
        fromGate = gate;

        // Create new GameObject for the runtime line
        GameObject lineObj = new GameObject("Wire");
        currentLine = lineObj.AddComponent<LineRenderer>();

        // Setup line appearance
        currentLine.material = new Material(Shader.Find("Sprites/Default"));
        currentLine.startColor = Color.yellow;
        currentLine.endColor = Color.yellow;
        currentLine.startWidth = 0.05f;
        currentLine.endWidth = 0.05f;
        currentLine.positionCount = 2;

        // Set start point to gate's connect point
        Vector3 start = gate.GetComponent<GateConnector>().connectPoint.position;
        start.z = 0;
        currentLine.SetPosition(0, start);
        currentLine.SetPosition(1, start); // initial, will move in Update
    }

    public void EndConnection(LogicGate toGate)
    {
        if (fromGate != null && toGate != null && fromGate != toGate)
        {
            // Connect gates logically
            toGate.AddInput(fromGate);

            // Finish the line visually
            Vector3 end = toGate.GetComponent<GateConnector>().connectPoint.position;
            end.z = 0;
            currentLine.SetPosition(1, end);
        }
        else
        {
            // Cancel, destroy line
            Destroy(currentLine.gameObject);
        }

        // Reset for next drag
        fromGate = null;
        currentLine = null;
    }
}
