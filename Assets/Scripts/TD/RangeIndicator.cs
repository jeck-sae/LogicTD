using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    public SpriteRenderer sr;
    public LineRenderer lr;
    
    public void SetColor(Color color)
    {
        Color srColor = color;
        srColor.a = sr.color.a;
        sr.color = srColor;

        Color lrColor = color;
        lrColor.a = lr.startColor.a;
        lr.startColor = lrColor;
        lr.endColor = lrColor;
    }
}
