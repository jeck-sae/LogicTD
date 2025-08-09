using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ConnectionLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public ConnectionPoint inputPoint;
    public ConnectionPoint outputPoint;

    private const float LineClickThreshold = 0.1f;

    public void Initialize(ConnectionPoint output, ConnectionPoint input)
    {
        lineRenderer = GetComponent<LineRenderer>();
        outputPoint = output;
        inputPoint = input;
    }

    private void Update()
    {
        if (lineRenderer != null && outputPoint != null && inputPoint != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, outputPoint.Position);
            lineRenderer.SetPosition(1, inputPoint.Position);
        }
    }

    public bool IsMouseNearLine(Vector3 mousePos)
    {
        if (outputPoint == null || inputPoint == null) return false;

        Vector3 p1 = outputPoint.Position;
        Vector3 p2 = inputPoint.Position;
        float distance = DistancePointToLine(mousePos, p1, p2);
        return distance <= LineClickThreshold;
    }

    private float DistancePointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        float length = Vector3.Distance(lineStart, lineEnd);
        if (length == 0f) return Vector3.Distance(point, lineStart);
        float t = Mathf.Clamp01(Vector3.Dot(point - lineStart, lineEnd - lineStart) / (length * length));
        Vector3 projection = lineStart + t * (lineEnd - lineStart);
        return Vector3.Distance(point, projection);
    }
}
