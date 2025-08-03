using System.Collections.Generic;

namespace TowerDefense
{
    public class LogicInput : LogicComponent
    {
        public bool value;

        public override int RequiredInputs => 0;

        public override bool Evaluate(List<bool> inputs)
        {
            return value;
        }
    }
}