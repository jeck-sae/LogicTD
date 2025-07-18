using UnityEngine;

public class NotGate : LogicGate
{
    public override void Evaluate()
    {
        if (inputs.Count < 1)
        {
            output = true;
            return;
        }

        inputs[0].Evaluate();
        output = !inputs[0].output;
    }
}
