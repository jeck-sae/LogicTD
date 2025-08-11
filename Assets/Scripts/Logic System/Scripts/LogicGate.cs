using UnityEngine;

public abstract class LogicGate : MonoBehaviour
{
    public ConnectionPoint[] inputs; // assign in inspector

    public bool output;

    public virtual void Evaluate()
    {
        bool[] inputValues = new bool[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            ConnectionPoint input = inputs[i];

            if (input.connectedLine != null)
            {
                ConnectionPoint connectedOutput = input.connectedLine.from;
                LogicGate connectedGate = connectedOutput.GetComponentInParent<LogicGate>();

                if (connectedGate != null)
                {
                    connectedGate.Evaluate();
                    inputValues[i] = connectedGate.output;
                }
                else
                {
                    inputValues[i] = false;
                }
            }
            else
            {
                inputValues[i] = false;
            }
        }

        output = CalculateOutput(inputValues);
    }

    protected abstract bool CalculateOutput(bool[] inputs);
}
