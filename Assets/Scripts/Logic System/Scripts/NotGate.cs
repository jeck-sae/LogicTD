using UnityEngine;

public class NotGate : LogicGate
{
    public override void Evaluate()
    {
        if (inputs.Count > 0)
        {
            inputs[0].Evaluate();
            output = !inputs[0].output;
        }
        else
        {
            output = true; // or false, depending how you want to handle no input
        }
    }
}
