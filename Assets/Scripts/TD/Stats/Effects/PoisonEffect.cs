using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TowerDefense
{
    public class PoisonEffect : DamageOverTImeEffect
    {
        public PoisonEffect(string effectName, EffectType type, Targetable target, float damagePerSecond) : base(effectName, type, target, damagePerSecond) { }
    
        public override void UpdateEffect()
        {
            float dmg = damagePerSecond / GameManager.gameTickFrequency;
    
            if (target.currentHealth > dmg)
                target.Damage(dmg);
            else
                target.Damage(target.currentHealth - 1);
        }
    }
}
