using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public class GateSlot : MonoBehaviour, ITowerSlot
    {
        private Tower tower;
        public Tower Tower => tower;
        [SerializeField, ReadOnly] private LogicComponent connectedGate;
        public bool canPlaceNewComponent;
        
        public List<GateSlot> inputs;
        [ReadOnly] public List<GateSlot> outputs;

        [ReadOnly] public bool state;

        private void Awake()
        {
            connectedGate = GetComponentInChildren<LogicComponent>();
            if (connectedGate) connectedGate.slot = this;
            
            foreach (var input in inputs)
                input.outputs.Add(this);
        }

        public void UpdateState()
        {
            state = connectedGate?.Evaluate(inputs.Select(x => x.state).ToList()) ?? false;
        }
        
        
        public bool CanPlace(Tower t)
        {
            return canPlaceNewComponent && !tower && t.TryGetComponent<LogicComponent>(out var c) && c.RequiredInputs == inputs.Count;
        }

        public void PlaceTower(Tower t)
        {
            if (!CanPlace(t))
                return;
            
            tower = t;
            t.gameObject.SetActive(true);
            t.gameObject.transform.position = transform.position;
            t.gameObject.transform.parent = transform;
            t.SetSlot(this);
            name = $"Gate Node [" + t.towerName + "]";
            
            connectedGate = t.GetComponent<LogicComponent>();
            connectedGate.slot = this;
        }

        public void RemoveTower()
        {
            connectedGate.slot = null;
            connectedGate = null;
            tower = null;
            name = "Empty Gate Node";
        }
    }
}
