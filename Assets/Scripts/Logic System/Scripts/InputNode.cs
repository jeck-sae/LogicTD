using UnityEngine;

public class InputNode : LogicGate
{
    public bool value;  // set this in Inspector or via UI

    public override void Evaluate()
    {
        output = value;
    }
}
