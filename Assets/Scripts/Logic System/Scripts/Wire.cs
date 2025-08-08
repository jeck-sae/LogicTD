using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(Collider2D))]
public class Wire : MonoBehaviour
{
    public ClickablePoint from;
    public ClickablePoint to;
    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;

        // Ensure collider is visible and clickable
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    public void SetConnection(ClickablePoint fromPoint, ClickablePoint toPoint)
    {
        from = fromPoint;
        to = toPoint;
        UpdateLine();
    }

    void Update()
    {
        UpdateLine();
    }

    void UpdateLine()
    {
        if (from && to)
        {
            Vector3 start = from.transform.position;
            Vector3 end = to.transform.position;
            line.SetPosition(0, start);
            line.SetPosition(1, end);

            // Update collider to match line
            UpdateCollider(start, end);
        }
    }

    void UpdateCollider(Vector3 start, Vector3 end)
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Vector3 midPoint = (start + end) / 2f;
        transform.position = midPoint;

        float length = Vector3.Distance(start, end);
        col.size = new Vector2(length, 0.1f);
        Vector3 direction = (end - start).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
