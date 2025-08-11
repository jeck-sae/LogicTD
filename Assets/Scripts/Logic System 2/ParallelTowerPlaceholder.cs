using Unity.VisualScripting;
using UnityEngine;

namespace TowerDefense
{
    public class ParallelTowerPlaceholder : Interactable2D
    {
        private ITowerSlot slot;
        
        public void Initialize(ITowerSlot slot)
        {
            this.slot = slot;
        }
        
        protected override void OnCursorExit()
        {
            if (enabled && Input.GetMouseButton(0) && slot.Tower)
                slot.Tower.StartMoving();
        }
    }
}