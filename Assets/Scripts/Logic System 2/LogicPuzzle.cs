using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace TowerDefense
{
    public class LogicPuzzle : MonoBehaviour
    {
        [SerializeField] protected List<PuzzleResult> outputs = new ();

        public bool CheckResult()
        {
            return outputs.All(x => x.expectedValue == x.outputGate.slot.state);
        }

        
        [System.Serializable]
        public class PuzzleResult
        {
            public bool expectedValue;
            public LogicOutput outputGate;
        }
    }
}
