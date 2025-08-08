using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance;

    private ClickablePoint selectedPoint;
    private List<Wire> wires = new List<Wire>();
    public GameObject wirePrefab;

    void Awake()
    {
        Instance = this;
    }

    public void PointClicked(ClickablePoint point)
    {
        if (selectedPoint == null)
        {
            selectedPoint = point;
        }
        else
        {
            // Prevent connecting input-to-input or output-to-output
            if (selectedPoint.isOutput == point.isOutput)
            {
                Debug.LogWarning("Cannot connect input to input or output to output.");
                selectedPoint = null;
                return;
            }

            // Ensure output connects to input
            ClickablePoint from = selectedPoint.isOutput ? selectedPoint : point;
            ClickablePoint to = selectedPoint.isOutput ? point : selectedPoint;

            // Remove existing wires going into 'to'
            foreach (var wire in new List<Wire>(wires))
            {
                if (wire.to == to)
                {
                    Destroy(wire.gameObject);
                    wires.Remove(wire);
                }
            }

            Wire wireObj = Instantiate(wirePrefab).GetComponent<Wire>();
            wireObj.SetConnection(from, to);
            wires.Add(wireObj);

            selectedPoint = null;
        }
    }

    void Update()
    {
        // Delete wire with right click
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Wire wire = hit.collider.GetComponent<Wire>();
                if (wire != null)
                {
                    wires.Remove(wire);
                    Destroy(wire.gameObject);
                }
            }
        }

        // Remove last wire with backspace
        if (Input.GetKeyDown(KeyCode.Backspace) && wires.Count > 0)
        {
            Wire lastWire = wires[wires.Count - 1];
            wires.RemoveAt(wires.Count - 1);
            Destroy(lastWire.gameObject);
        }
    }
}
