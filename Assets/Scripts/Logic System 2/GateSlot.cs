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
        
        [SerializeReference] public List<GateConnection> inputs = new ();
        [ReadOnly] public List<GateSlot> outputs;

        [ReadOnly] public bool state;

        public event Action<bool> OnStateChanged;
        public event Action<bool> OnTowerPlacedOrRemoved;
        
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
                go.transform.SetParent(transform);
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

        List<(Transform transform, bool state)> previousGFXStates = new();
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

            var gfx = tower.transform.Find("GFX");
            for (int i = 0; i < gfx.childCount; i++)
            {
                var child = gfx.GetChild(i);
                if (child.name == "Base")
                    continue;
                
                previousGFXStates.Add((child, child.gameObject.activeSelf));
                child.gameObject.SetActive(false);
            }
            
            connectedGate = t.GetComponent<LogicComponent>();
            connectedGate.slot = this;

            LogicManager.Instance.UpdateStates();
            OnTowerPlacedOrRemoved?.Invoke(true);
        }

        public void RemoveTower()
        {
            connectedGate.slot = null;
            connectedGate = null;
            tower = null;
            name = "Empty Gate Node";

            previousGFXStates.ForEach(x 
                => x.transform?.gameObject.SetActive(x.state));
            
            LogicManager.Instance.UpdateStates();
            OnTowerPlacedOrRemoved?.Invoke(false);
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
        
        
        
        // ################### Gizmos
        private void OnDrawGizmosSelected()
        {
            DrawGizmos(true);
        }
        private void OnDrawGizmos()
        {
            DrawGizmos(false);
        }
        void DrawGizmos(bool selected)
        {
            foreach (var input in inputs)
            {
                if (input == null || !input.from)
                    return;
                
                var xDiff = transform.position.x - input.from.transform.position.x;
                var midX = input.from.transform.position.x + xDiff * input.verticalOffset;

                if (selected)
                {
                    Gizmos.color = new Color(.3f, .3f, .3f);
                    Gizmos.DrawLine(
                        new Vector3(transform.position.x, transform.position.y),
                        new Vector3(input.from.transform.position.x, input.from.transform.position.y));
                }

                Gizmos.color = selected ? Color.green : new Color(1, 0, 0, .5f);
                Gizmos.DrawLine(
                    new Vector3(midX, input.from.transform.position.y),
                    new Vector3(midX, transform.position.y));
                Gizmos.DrawSphere(new Vector3(midX, transform.position.y), .075f);
            }
        }
    }
}
