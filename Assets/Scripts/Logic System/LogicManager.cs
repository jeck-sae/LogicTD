using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public List<LogicGate> gates = new List<LogicGate>();

    void Update()
    {
        foreach (var gate in gates)
        {
            gate.Evaluate();
        }
    }
}
