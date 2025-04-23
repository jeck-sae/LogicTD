using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class StatisticsTracker : MonoBehaviour
    {
        [SerializeField]
        GameStats stats = new();

        private void Start()
        {
            GameEvents.OnDamageDealt += OnDamageDealt;
            GameEvents.OnEnemyKilled += OnEnemyKilled;
            GameEvents.OnTowerPlayed += OnTowerPlayed;
        }

        private void OnDestroy()
        {
            GameEvents.OnDamageDealt -= OnDamageDealt;
            GameEvents.OnEnemyKilled -= OnEnemyKilled;
            GameEvents.OnTowerPlayed -= OnTowerPlayed;
        }

        void OnTowerPlayed(Tower tower)
        {
            if (!stats.towersPlayed.ContainsKey(tower.towerName))
                stats.towersPlayed.Add(tower.towerName, 0);
            stats.towersPlayed[tower.towerName] += 1;
        }

        void OnEnemyKilled(Targetable target, Tower attacker)
        {
            stats.enemiesKilled++;
        }

        void OnDamageDealt(Targetable targetable, float damage, Tower attacker)
        {
            stats.totalDamage += damage;
            if (attacker)
            {
                if (!stats.towerDamage.ContainsKey(attacker.towerName))
                    stats.towerDamage.Add(attacker.towerName, 0);
                stats.towerDamage[attacker.towerName] += damage;
            }
        }

        [Serializable]
        public class GameStats
        {
            public float totalDamage;
            public int enemiesKilled;
            [ShowInInspector] public Dictionary<string, float> towerDamage = new();            
            [ShowInInspector] public Dictionary<string, int> towersPlayed = new();
        }
    }
}
