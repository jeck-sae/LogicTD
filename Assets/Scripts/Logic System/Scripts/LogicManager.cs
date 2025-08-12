using UnityEngine;
using System.Collections.Generic;

public class LogicManager : MonoBehaviour
{
    public List<OutputNode> outputNodes;

    public void EvaluateAll()
    {
        foreach (var node in outputNodes)
        {
            node.Evaluate();
        }
    }

    public bool IsSolutionCorrect()
    {
        foreach (var node in outputNodes)
        {
            if (!node.isCorrect)
                return false;
        }
        return true;
    }
}