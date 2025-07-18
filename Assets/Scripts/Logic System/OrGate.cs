using UnityEngine;

public class OrGate : LogicGate
{
    public override void Evaluate()
    {
        output = false;
        foreach (var gate in inputs)
        {
            gate.Evaluate();
            if (gate.output)
            {
                output = true;
                break;
            }
        }
    }
}
