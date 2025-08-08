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
        public event Action OnPuzzleUnsolved;
        public bool allSlotsFilled;
        public bool solved;
        private GateSlot[] gates;
        
        private void Start()
        {
            outputs.ForEach(x => x.outputGate.slot.OnStateChanged += CheckResult);
            gates = GetComponentsInChildren<GateSlot>();
        }

        private void CheckResult(bool obj)
        {
            bool before = allSlotsFilled && solved;
            allSlotsFilled = gates.All(x => !x.canPlaceNewComponent || x.Tower);
            solved = outputs.All(x => x.outputGate.slot.state == x.expectedValue);
            bool after = allSlotsFilled && solved;
            
            if(before && !after) OnPuzzleUnsolved?.Invoke();
            if(after) OnPuzzleSolved?.Invoke();
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
