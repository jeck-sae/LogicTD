using UnityEngine;

namespace TowerDefense
{
    public class SlowTimeOnPuzzle : MonoBehaviour
    {
        [SerializeField] private Transform bottomLeft;
        [SerializeField] private Transform topRight;

        bool inside = false;
        public void Update()
        {
            var mousePosition = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.x > bottomLeft.position.x && mousePosition.x < topRight.position.x 
             && mousePosition.y > bottomLeft.position.y && mousePosition.y < topRight.position.y)
            {
                if (inside)
                    return;
                inside = true;
                OnCursorInside();
                return;
            }

            if (!inside)
                return;
            inside = false;
            OnCursorOutside();
        }


        protected void OnCursorInside()
        {
            Time.timeScale /= 4;
        }

        protected void OnCursorOutside()
        {
            Time.timeScale *= 4;
        }

    }
}
