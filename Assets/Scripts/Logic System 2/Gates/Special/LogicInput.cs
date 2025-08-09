using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public class LogicInput : LogicComponent
    {
        [OnValueChanged(nameof(UpdateColor))]
        public bool inputState;

        [ColorPalette] public Color onColor;
        [ColorPalette] public Color offColor;
        
        private void UpdateColor()
        {
            if(TryGetComponent(out SpriteRenderer sr))
                sr.color = inputState ? onColor : offColor;
        }
        
        public override int RequiredInputs => 0;

        public override bool Evaluate(List<bool> inputs)
        {
            return inputState;
        }
    }
}