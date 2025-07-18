using UnityEngine;

public class AndGate : LogicGate
{
    public override void Evaluate()
    {
        if (inputs.Count < 2)
        {
            output = false;
            return;
        }

        output = true;
        foreach (var gate in inputs)
        {
            gate.Evaluate();
            if (!gate.output)
            {
                output = false;
                break;
            }
        }
    }
}
