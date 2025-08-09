using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    private LogicGate selectedOutputGate = null;
    private Transform selectedOutputPoint = null;

    // List to keep track of all wires created
    private List<GameObject> wireObjects = new List<GameObject>();

    void Update()
    {
        // Check if Backspace is pressed
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveLastWire();
        }
    }

    public void OnClickConnectionPoint(LogicGate gate, Transform point, bool isOutput)
    {
        if (isOutput)
        {
            selectedOutputGate = gate;
            selectedOutputPoint = point;
            Debug.Log("Selected output from gate: " + gate.name);
        }
        else
        {
            if (selectedOutputGate != null && selectedOutputGate != gate)
            {
                ConnectGates(selectedOutputGate, selectedOutputPoint, gate, point);
                ClearSelection();
            }
            else
            {
                Debug.Log("No valid output selected or cannot connect gate to itself.");
            }
        }
    }

    private void ConnectGates(LogicGate fromGate, Transform fromPoint, LogicGate toGate, Transform toPoint)
    {
        toGate.AddInput(fromGate);

        GameObject lineObj = new GameObject("Wire");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.yellow;
        lr.endColor = Color.yellow;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.positionCount = 2;

        Vector3 start = fromPoint.position;
        Vector3 end = toPoint.position;
        start.z = 0;
        end.z = 0;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lineObj.AddComponent<Wire>();
        BoxCollider2D collider = lineObj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        Vector3 midPoint = (start + end) / 2f;
        lineObj.transform.position = midPoint;

        float length = Vector3.Distance(start, end);
        collider.size = new Vector2(length, 0.1f);

        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
        lineObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Add this wire to our list
        wireObjects.Add(lineObj);

        Debug.Log("Connected " + fromGate.name + " output to " + toGate.name + " input.");
    }

    private void ClearSelection()
    {
        selectedOutputGate = null;
        selectedOutputPoint = null;
    }

    private void RemoveLastWire()
    {
        if (wireObjects.Count > 0)
        {
            GameObject lastWire = wireObjects[wireObjects.Count - 1];
            wireObjects.RemoveAt(wireObjects.Count - 1);
            Destroy(lastWire);
            Debug.Log("Last wire removed");
        }
        else
        {
            Debug.Log("No wires to remove");
        }
    }
}
