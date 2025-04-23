using Sirenix.OdinInspector;
using Sirenix.Utilities;
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

            var components = GetComponents<IStatComponent>();
            foreach (var component in components)
            {
                var s = component.GetStats();
                s.stats.ForEach(x => tempStats.AddStat(x.Value.Name, x.Value));
            }

            return tempStats;
        }
    
        protected void OnMaxHealthChanged(Stat.StatValueChangedEventArgs args)
        {
            currentHealth = currentHealth / args.previousValue * args.newValue;
        }
    
        public void Damage(float amount, Tower attacker = null)
        {
            if (amount <= 0)
                return;
    
            float damage = amount * stats["damageTakenMultiplier"];
            currentHealth -= damage;
    
            HealthChanged?.Invoke();
            GameEvents.DamageDealt(this, damage, attacker);

            if (currentHealth < .001f)
                Kill(attacker);
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

        public void Kill(Tower attacker = null) => Kill(false, attacker);
        protected void Kill(bool reachedHomeTile, Tower attacker = null)
        {
            isAlive = false;
            OnDeath?.Invoke(new OnDeathArgs() { reachedHomeTile = reachedHomeTile });
            GameEvents.EnemyKilled(this, attacker);
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
