using Febucci.UI.Core.Parsing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense
{
    [RequireComponent (typeof (IStatObject))]
    public class HealNearbyEnemies : MonoBehaviour
    {
        public float addPercent;
        public float addMissingPercent;

        public Stat HealEffectiveness;
        public Stat HealFrequency;
        public Stat HealRange;

        Stats stats;
        private void Awake()
        {
            stats = GetComponent<IStatObject>()?.GetStats();
            stats.AddStat("healRange", HealFrequency);
            stats.AddStat("healFrequency", HealFrequency);
            stats.AddStat("healEffectiveness", HealEffectiveness);
        }

        float nextHealTime;
        private void Update()
        {
            if(nextHealTime < Time.time)
            {
                nextHealTime = Time.time + 1 / HealFrequency;

                var hit = Physics2D.CircleCastAll(transform.position, HealRange,
                    Vector2.zero, 0, layerMask: LayerMask.GetMask("Targetable"));
                var enemies = hit.Select(x => x.collider.GetComponent<Enemy>()).Where(x => x != null && x.isAlive);

                foreach (Enemy enemy in enemies)
                {
                    float perc = enemy.currentHealth / enemy.MaxHealth;
                    enemy.Heal(addPercent * enemy.MaxHealth * HealEffectiveness);
                    enemy.Heal(enemy.MaxHealth * (1 - perc) * addMissingPercent * HealEffectiveness);
                }
            }
        }
    }
}
