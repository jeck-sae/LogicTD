using UnityEngine;


namespace TowerDefense
{
    public class RangeIndicator : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sr;
        [SerializeField] LineRenderer lr;
        public ScaleWithStat scaleWithStat;
    
    
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
    
}
