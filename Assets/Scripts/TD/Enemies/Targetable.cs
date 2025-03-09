using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace TowerDefense
{
    public class Targetable : ManagedBehaviour, IStatObject
    {
        [HideInEditorMode]
        public float currentHealth = 0;
        public bool isAlive = true;
    
        public Stat MaxHealth;
        public Stat DamageTakenMultiplier;
    
        public event Action HealthChanged;
    
        [HideInInspector]
        public Stats stats;
    
    
        protected override void ManagedInitialize()
        {
            stats = GetStats();
    
            currentHealth = MaxHealth;
            MaxHealth.OnValueChanged += OnMaxHealthChanged;
        }
    
        public virtual Stats GetStats() 
        {
            if (stats != null) return stats;
    
            var tempStats = new Stats();
            tempStats.AddStat("maxHealth", MaxHealth);
            tempStats.AddStat("damageTakenMultiplier", DamageTakenMultiplier, 0);
            return tempStats;
        }
    
        protected void OnMaxHealthChanged(Stat.StatValueChangedEventArgs args)
        {
            currentHealth = currentHealth / args.previousValue * args.newValue;
        }
    
        public void Damage(float amount)
        {
            if (amount <= 0)
                return;
    
            currentHealth -= amount * stats["damageTakenMultiplier"];
    
            HealthChanged?.Invoke();
    
            if (currentHealth <= 0)
                isAlive = false;
        }
    
        public void Heal(float amount)
        {
            if (amount <= 0)
                return;
    
            currentHealth += amount;
    
            HealthChanged?.Invoke();
    
            if (currentHealth > MaxHealth)
                currentHealth = MaxHealth;
        }
    
    
    
        protected virtual void Kill()
        {
            Destroy(gameObject);
        }
    }
}
