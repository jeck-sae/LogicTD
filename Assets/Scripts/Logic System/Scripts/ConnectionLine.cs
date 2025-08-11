using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(BoxCollider2D))]
public class ConnectionLine : MonoBehaviour
{
    public ConnectionPoint from; // output point
    public ConnectionPoint to;   // input point

    private LineRenderer lineRenderer;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        lineRenderer.positionCount = 2;
        boxCollider.isTrigger = true;
    }

    public void Initialize(ConnectionPoint outputPoint, ConnectionPoint inputPoint)
    {
        from = outputPoint;
        to = inputPoint;

        // Assign this line to input's connectedLine
        if (inputPoint.connectedLine != null)
        {
            Destroy(inputPoint.connectedLine.gameObject);
        }
        inputPoint.connectedLine = this;

        UpdateLine();
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        if (from == null || to == null) return;

        Vector3 startPos = from.transform.position;
        Vector3 endPos = to.transform.position;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        UpdateCollider(startPos, endPos);
    }

    private void UpdateCollider(Vector3 start, Vector3 end)
    {
        Vector3 midPoint = (start + end) / 2f;
        transform.position = midPoint;

        float length = Vector3.Distance(start, end);
        boxCollider.size = new Vector2(length, 0.1f);

        Vector3 direction = (end - start).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public bool IsMouseNearLine(Vector3 mousePos, float threshold = 0.2f)
    {
        if (from == null || to == null) return false;

        Vector3 start = from.transform.position;
        Vector3 end = to.transform.position;

        float length = Vector3.Distance(start, end);
        if (length == 0) return false;

        Vector3 dir = (end - start).normalized;
        float proj = Vector3.Dot(mousePos - start, dir);
        proj = Mathf.Clamp(proj, 0, length);

        Vector3 closestPoint = start + dir * proj;
        float distToLine = Vector3.Distance(mousePos, closestPoint);

        return distToLine <= threshold;
    }

    private void OnDestroy()
    {
        if (to != null && to.connectedLine == this)
        {
            to.connectedLine = null;
        }
    }
}
