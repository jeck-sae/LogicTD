using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public class ConnectionWire : MonoBehaviour
    {
        private GateSlot from;
        private GateSlot to;
        private LineRenderer lr;

        private float fromY;
        private float toY;
        private float offset;
        private static GlobalWireSettings s => GlobalWireSettings.Instance; 
        
        public void Initialize(GateSlot from, GateSlot to, float offset)
        {
            this.from = from;
            this.to = to;
            this.offset = offset;
            
            lr = gameObject.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            
            DrawFullLine();
            
            from.OnStateChanged += UpdateColor;
            UpdateColor(from.state);
        }

        [Button]
        void DrawFullLine()
        {
            int fromPosition = Array.IndexOf(from.outputs.OrderBy(x => x.transform.position.y).ToArray(), to);
            int fromTotal = from.outputs.Count;

            int toPosition = to.inputs.FindIndex(x => x.from == from);
            int toTotal = to.inputs.Count;
            toPosition = toTotal - 1 - toPosition;

            float fromYOffset = s.verticalConnectionArea / (fromTotal + 1) * (fromPosition + 1) - 
                                s.verticalConnectionArea / 2;
            float toYOffset = s.verticalConnectionArea / (toTotal + 1) * (toPosition + 1) - 
                              s.verticalConnectionArea / 2;
            
            fromY = from.transform.position.y + fromYOffset;
            toY = to.transform.position.y + toYOffset;
            
            var p0 = new Vector3(from.transform.position.x + s.distanceFromWire, fromY);
            var p3 = new Vector3(to.transform.position.x - s.distanceFromWire, toY);
            
            lr.widthMultiplier = s.width;
            lr.positionCount = 4;
            
            lr.SetPosition(0, p0);
            lr.SetPosition(3, p3);
            
            SetOffset(offset);
        }
        public void SetOffset(float offset)
        {
            this.offset = offset;
            
            var xDiff = to.transform.position.x - from.transform.position.x;
            var midX = from.transform.position.x + xDiff * offset;
            
            var p1 = new Vector3(midX, fromY);
            var p2 = new Vector3(midX, toY);
            
            lr.SetPosition(1, p1);
            lr.SetPosition(2, p2);
        }

        public void UpdateColor(bool state)
        {
            lr.startColor = state ? s.onColor : s.offColor;
            lr.endColor = state ? s.onColor : s.offColor;
        }
        
        private void OnDestroy()
        {
            if(from) from.OnStateChanged -= UpdateColor;
        }
    }
}