using UnityEngine;

public class Wire : MonoBehaviour
{
    private bool isHovered = false;
    private LineRenderer lineRenderer;
    private BoxCollider2D boxCollider;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        if (lineRenderer == null)
        {
            Debug.LogWarning("Wire requires a LineRenderer component.");
        }
        if (boxCollider == null)
        {
            Debug.LogWarning("Wire requires a BoxCollider2D component.");
        }
    }

    void Update()
    {
        if (lineRenderer == null || boxCollider == null) return;

        // Update collider to match line
        Vector3 start = lineRenderer.GetPosition(0);
        Vector3 end = lineRenderer.GetPosition(1);
        Vector3 midPoint = (start + end) / 2f;

        transform.position = midPoint;

        float length = Vector3.Distance(start, end);
        boxCollider.size = new Vector2(length, 0.1f);

        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Check Delete key while hovering
        if (isHovered && Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(gameObject);
        }
    }

    void OnMouseEnter()
    {
        isHovered = true;
        if (lineRenderer != null)
            lineRenderer.startColor = lineRenderer.endColor = Color.red;
    }

    void OnMouseExit()
    {
        isHovered = false;
        if (lineRenderer != null)
            lineRenderer.startColor = lineRenderer.endColor = Color.yellow;
    }
}
