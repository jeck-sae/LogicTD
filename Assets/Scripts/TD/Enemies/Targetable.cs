using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace TowerDefense
{
    public class Targetable : ManagedBehaviour, IStatObject
    {
        [HideInEditorMode]
        public float currentHealth = 0;
        public bool isAlive { private set; get; } = true;
    
        public Stat MaxHealth;
        public Stat DamageTakenMultiplier;
    
        public event Action HealthChanged;
        public event Action<OnDeathArgs> OnDeath;
    
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
    
            if (currentHealth < .001f)
                Kill();
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

        public void Kill() => Kill(false);
        protected void Kill(bool reachedHomeTile)
        {
            isAlive = false;
            OnDeath?.Invoke(new OnDeathArgs() { reachedHomeTile = reachedHomeTile });
            OnKill();
        }

        protected virtual void OnKill()
        {
            Debug.Log(name);
            Destroy(gameObject);
        }

        public class OnDeathArgs
        {
            public bool reachedHomeTile;
        }
    }
}
