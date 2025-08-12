using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance;

    public GameObject linePrefab;

    private ConnectionPoint firstPoint;

    private List<ConnectionLine> lines = new List<ConnectionLine>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PointClicked(ConnectionPoint point)
    {
        if (firstPoint == null)
        {
            firstPoint = point;
        }
        else
        {
            if (firstPoint.isOutput != point.isOutput)
            {
                // Ensure output is first argument
                ConnectionPoint output = firstPoint.isOutput ? firstPoint : point;
                ConnectionPoint input = firstPoint.isInput ? firstPoint : point;

                // Remove old connection to input if exists
                if (input.connectedLine != null)
                {
                    Destroy(input.connectedLine.gameObject);
                    input.connectedLine = null;
                }

                CreateConnection(output, input);
            }
            else
            {
                Debug.LogWarning("Cannot connect two inputs or two outputs!");
            }

            firstPoint = null;
        }
    }

    private void CreateConnection(ConnectionPoint output, ConnectionPoint input)
    {
        GameObject lineObj = Instantiate(linePrefab);
        ConnectionLine line = lineObj.GetComponent<ConnectionLine>();
        line.Initialize(output, input);
        lines.Add(line);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click to delete line
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            foreach (var line in new List<ConnectionLine>(lines))
            {
                if (line.IsMouseNearLine(mousePos))
                {
                    lines.Remove(line);
                    Destroy(line.gameObject);
                    break;
                }
            }
        }
    }
}