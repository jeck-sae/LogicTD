using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TowerDefense
{
    public class DamageOverTImeEffect : Effect
    {
        public Targetable target;
        public float damagePerSecond;
    
        public DamageOverTImeEffect(string name, EffectType type, Targetable target, float damagePerSecond) : base(name, type)
        {
            this.target = target;
            this.damagePerSecond = damagePerSecond;
        }
    
        public override void UpdateEffect()
        {
            target.Damage(damagePerSecond / GameManager.gameTickFrequency);
        }
    
    }
}
