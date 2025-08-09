using UnityEngine;

public class AndGate : LogicGate
{
    public override void Evaluate()
    {
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
