using System;
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

        public event Action OnPuzzleSolved;
        private bool solved;
        
        private void Start()
        {
            outputs.ForEach(x => x.outputGate.slot.OnStateChanged += CheckResult);
        }

        private void CheckResult(bool obj)
        {
            if(!solved && outputs.All(x => x.expectedValue == x.outputGate.slot.state))
                OnPuzzleSolved?.Invoke();
        }

        private void OnDestroy()
        {
            foreach (var output in outputs)
            {
                if(!output.outputGate || !output.outputGate.slot)
                    continue;
                output.outputGate.slot.OnStateChanged -= CheckResult;
            }
        }

        [Serializable]
        public class PuzzleResult
        {
            public bool expectedValue;
            public LogicOutput outputGate;
        }
    }
}
