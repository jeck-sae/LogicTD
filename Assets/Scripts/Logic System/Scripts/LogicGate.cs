using System.Collections.Generic;
using UnityEngine;

public abstract class LogicGate : MonoBehaviour
{
    public List<LogicGate> inputs = new List<LogicGate>();
    public bool output;

    public void AddInput(LogicGate gate)
    {
        if (!inputs.Contains(gate))
            inputs.Add(gate);
    }

    public abstract void Evaluate();
}
