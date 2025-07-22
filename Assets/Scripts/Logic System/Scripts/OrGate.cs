using UnityEngine;

public class OrGate : LogicGate
{
    public override void Evaluate()
    {
        output = false; // start with false
        foreach (var gate in inputs)
        {
            gate.Evaluate();
            if (gate.output)
            {
                output = true;
                break; // if any input is true, we're done
            }
        }
    }
}
