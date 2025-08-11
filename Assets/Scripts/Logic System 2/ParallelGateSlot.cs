using Sirenix.Utilities;
using UnityEngine;

namespace TowerDefense
{
    public class ParallelGateSlot : GateSlot
    {
        protected GameObject parallelGfx;
        protected ParallelTowerPlaceholder placeholder;
        
        protected override void Awake()
        {
            base.Awake();
            GameObject go = new GameObject("PlaceholderTower");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.AddComponent<BoxCollider2D>();
            placeholder = go.AddComponent<ParallelTowerPlaceholder>();
            placeholder.Initialize(this);
            placeholder.enabled = false;
        }
        
        public void ParallelRemove()
        {
            if(parallelGfx)
                Destroy(parallelGfx);
            connectedGate = null;
            tower = null;
            name = "Empty Parallel Gate Node";
            
            placeholder.enabled = false;
            LogicManager.Instance.UpdateStates();
        }
        public void ParallelPlace(Tower t)
        {
            tower = t;
            var gfx = t.transform.Find("GFX");
            parallelGfx = Instantiate(gfx, transform).gameObject;
            name = $"Parallel Gate Node [" + t.towerName + "]";
            placeholder.enabled = true;
            
            connectedGate = t.GetComponent<LogicComponent>();
            LogicManager.Instance.UpdateStates();
        }
        
        public override void PlaceTower(Tower t)
        {
            base.PlaceTower(t);
            t.SetSlot(this);
            var parallel = FindObjectsByType<ParallelGateSlot>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            parallel.ForEach(x => {
                if(x != this)
                    x.ParallelPlace(t);
            });
            name = $"Main Parallel Gate Node [" + t.towerName + "]";
        }

        public override void RemoveTower()
        {
            Debug.Log("REMOVE");
            var parallel = FindObjectsByType<ParallelGateSlot>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            parallel.ForEach(x => {
                x.ParallelRemove();
            });
        }
    }
}