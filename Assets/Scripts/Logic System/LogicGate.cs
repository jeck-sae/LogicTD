using System.Collections.Generic;
using UnityEngine;

public abstract class LogicGate : MonoBehaviour
{
    // List of input gates connected to this gate
    public List<LogicGate> inputs = new List<LogicGate>();

    // The output value of this gate
    [HideInInspector]
    public bool output;

    // Evaluate the gate's logic
    public abstract void Evaluate();

    // Add an input connection
    public void AddInput(LogicGate inputGate)
    {
        if (!inputs.Contains(inputGate))
            inputs.Add(inputGate);
    }
}
