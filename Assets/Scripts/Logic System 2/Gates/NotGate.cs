using System.Collections.Generic;

namespace TowerDefense
{
    public class NotGate : LogicComponent
    {
        public override int RequiredInputs => 1;
        public override bool Evaluate(List<bool> inputs)
        {
            return !inputs[0];
        }
    }
}