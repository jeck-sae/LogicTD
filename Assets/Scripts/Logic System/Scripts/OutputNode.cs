using UnityEngine;
using static UnityEditor.Rendering.CameraUI;
using UnityEngine.Windows;

public class OutputNode : LogicGate
{
    public bool expectedValue;
    public bool isCorrect;

    public override void Evaluate()
    {
        if (inputs != null && inputs.Length > 0)
        {
            if (inputs[0].connectedLine != null)
            {
                ConnectionPoint connectedOutput = inputs[0].connectedLine.from;
                LogicGate connectedGate = connectedOutput.GetComponentInParent<LogicGate>();
                if (connectedGate != null)
                {
                    connectedGate.Evaluate();
                    output = connectedGate.output;
                    isCorrect = (output == expectedValue);
                    return;
                }
            }
        }

        output = false;
        isCorrect = (output == expectedValue);
    }

    protected override bool CalculateOutput(bool[] inputs)
    {
        return output;
    }
}
