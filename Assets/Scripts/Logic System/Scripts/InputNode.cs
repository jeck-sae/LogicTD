using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public class InputNode : LogicGate
{
    public bool value;

    public override void Evaluate()
    {
        output = value;
    }

    protected override bool CalculateOutput(bool[] inputs)
    {
        return value;
    }
}