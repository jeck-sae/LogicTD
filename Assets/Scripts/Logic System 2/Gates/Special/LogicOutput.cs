using System.Collections.Generic;

namespace TowerDefense
{
    public class LogicOutput : LogicComponent
    {
        public override int RequiredInputs => 1;
        public override bool Evaluate(List<bool> inputs)
        {
            return inputs[0];
        }
    }
}