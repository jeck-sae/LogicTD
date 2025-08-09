using System.Collections.Generic;

namespace TowerDefense
{
    public class NandGate : LogicComponent
    {
        public override int RequiredInputs => 2;
        public override bool Evaluate(List<bool> inputs)
        {
            return !(inputs[0] & inputs[1]);
        }
    }
}