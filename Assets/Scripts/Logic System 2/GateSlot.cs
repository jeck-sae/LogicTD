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
        
        public List<GateConnection> inputs;
        [ReadOnly] public List<GateSlot> outputs;

        [ReadOnly] public bool state;

        public event Action<bool> OnStateChanged;
        
        private void Awake()
        {
            connectedGate = GetComponentInChildren<LogicComponent>();
            if (connectedGate) connectedGate.slot = this;
            
            foreach (var input in inputs)
                input.from.outputs.Add(this);
        }

        private void Start()
        {
            foreach (var input in inputs)
            {
                GameObject go = new GameObject("Wire");
                input.wire = go.AddComponent<ConnectionWire>();
                input.wire.Initialize(input.from, this, input.verticalOffset);
            }
        }

        public void UpdateState()
        {
            var previous = state;
            state = connectedGate?.Evaluate(
                inputs.Select(x => x.from.state).ToList()) ?? false;
            
            if (previous != state) 
                OnStateChanged?.Invoke(state);
        }
        
        
        public bool CanPlace(Tower t)
        {
            return canPlaceNewComponent && !tower && t.TryGetComponent<LogicComponent>(
                out var c) && c.RequiredInputs == inputs.Count;
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
        
        [Serializable]
        public class GateConnection
        {
            public GateSlot from;
            [OnValueChanged(nameof(OffsetChanged))]
            [Range(0, 1)] public float verticalOffset = .5f;
            [HideInEditorMode, DisableInPlayMode] public ConnectionWire wire;

            private void OffsetChanged()
            {
                wire?.SetOffset(verticalOffset);
            }
        }

    }
}
