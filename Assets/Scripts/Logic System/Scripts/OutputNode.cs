using UnityEngine;

public class OutputNode : LogicGate
{
    public bool expectedValue; // designer sets what should be the correct result
    public bool isCorrect;

    public override void Evaluate()
    {
        if (inputs.Count > 0)
        {
            inputs[0].Evaluate();
            output = inputs[0].output;
            isCorrect = (output == expectedValue);
        }
    }
}
