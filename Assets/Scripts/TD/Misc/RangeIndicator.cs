using UnityEngine;


namespace TowerDefense
{
    public class RangeIndicator : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sr;
        [SerializeField] LineRenderer lr;
        float maxSpriteOpacity;
        float maxLineOpacity;
        
        public ScaleWithStat scaleWithStat;

        private void Awake()
        {
            maxSpriteOpacity = sr.color.a;
            maxLineOpacity =  lr.startColor.a;
        }

        public void ShowColor(Color color)
        {
            gameObject.SetActive(true);

            Color srColor = color;
            srColor.a = color.a * maxSpriteOpacity;
            sr.color = srColor;
    
            Color lrColor = color;
            lrColor.a = color.a * maxLineOpacity;
            lr.startColor = lrColor;
            lr.endColor = lrColor;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
    
}
