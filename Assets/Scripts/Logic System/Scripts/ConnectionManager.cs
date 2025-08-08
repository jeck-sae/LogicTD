using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance;

    public GameObject linePrefab;

    private ConnectionPoint selectedOutput;

    private List<ConnectionLine> lines = new List<ConnectionLine>();

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && lines.Count > 0)
        {
            Destroy(lines[^1].gameObject);
            lines.RemoveAt(lines.Count - 1);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            foreach (var line in lines)
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

    public void HandleClick(ConnectionPoint point)
    {
        if (point.isOutput)
        {
            selectedOutput = point;
        }
        else
        {
            if (selectedOutput == null) return;

            // Prevent output-to-output
            if (!point.isOutput)
            {
                // Remove existing line going to this input
                ConnectionLine existing = lines.Find(l => l.input == point);
                if (existing != null)
                {
                    Destroy(existing.gameObject);
                    lines.Remove(existing);
                }

                GameObject lineGO = Instantiate(linePrefab);
                ConnectionLine line = lineGO.GetComponent<ConnectionLine>();
                line.Initialize(selectedOutput, point);
                lines.Add(line);
                selectedOutput = null;
            }
        }
    }
}
