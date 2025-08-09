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
            // Prevent connecting output to output or input to input
            if (firstPoint.isOutput != point.isOutput)
            {
                CreateConnection(firstPoint.isOutput ? firstPoint : point,
                                 firstPoint.isInput ? firstPoint : point);
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
        if (Input.GetMouseButtonDown(1)) // Right-click to delete a line
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            foreach (var line in new List<ConnectionLine>(lines))
            {
                if (line.IsMouseNearLine(mousePos))
                {
                    Destroy(line.gameObject);
                    lines.Remove(line);
                    break;
                }
            }
        }
    }
}
