using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public abstract class LogicComponent : MonoBehaviour
    {
        [ShowInInspector] public abstract int RequiredInputs { get; }
        [ReadOnly] public GateSlot slot;
        public abstract bool Evaluate(List<bool> inputs);
    }
}