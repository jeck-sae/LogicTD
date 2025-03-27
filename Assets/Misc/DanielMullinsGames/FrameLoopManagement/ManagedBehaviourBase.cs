using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TowerDefense
{
    public class ManagedBehaviourBase : MonoBehaviour
    {
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
        protected virtual void Awake() { }
    }
    
}
