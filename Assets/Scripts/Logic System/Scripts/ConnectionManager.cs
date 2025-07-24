using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public LogicGate fromGate;
    public Transform fromPoint;
    private LineRenderer currentLine;

    void Update()
    {
        if (currentLine != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            currentLine.SetPosition(1, mousePos);
        }
    }

    public void StartConnectionFromPoint(LogicGate gate, Transform point)
    {
        fromGate = gate;
        fromPoint = point;

        GameObject lineObj = new GameObject("Wire");
        currentLine = lineObj.AddComponent<LineRenderer>();
        currentLine.material = new Material(Shader.Find("Sprites/Default"));
        currentLine.startColor = Color.yellow;
        currentLine.endColor = Color.yellow;
        currentLine.startWidth = 0.05f;
        currentLine.endWidth = 0.05f;
        currentLine.positionCount = 2;

        Vector3 start = point.position;
        start.z = 0;
        currentLine.SetPosition(0, start);
        currentLine.SetPosition(1, start);
    }

    public void EndConnectionAtPoint(LogicGate toGate, Transform toPoint)
    {
        if (fromGate != null && toGate != null && fromGate != toGate)
        {
            toGate.AddInput(fromGate);
            Vector3 end = toPoint.position;
            end.z = 0;
            currentLine.SetPosition(1, end);
        }
        else
        {
            Destroy(currentLine.gameObject);
        }

        fromGate = null;
        fromPoint = null;
        currentLine = null;
    }
}
