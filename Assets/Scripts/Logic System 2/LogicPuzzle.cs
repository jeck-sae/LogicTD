using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace TowerDefense
{
    public class LogicPuzzle : MonoBehaviour
    {
        [SerializeField] protected List<PuzzleResult> outputs = new ();

        public event Action OnPuzzleSolved;
        public event Action OnPuzzleUnsolved;
        [ReadOnly] public bool allSlotsFilled;
        [ReadOnly] public bool solved;
        private GateSlot[] gates;
        
        private void Start()
        {
            gates = GetComponentsInChildren<GateSlot>();
            gates.ForEach(x => x.OnTowerPlacedOrRemoved += CheckResult);
        }

        private void CheckResult(bool placed)
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
                output.outputGate.slot.OnTowerPlacedOrRemoved -= CheckResult;
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
