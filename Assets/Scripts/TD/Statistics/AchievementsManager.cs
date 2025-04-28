using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public class AchievementsManager : MonoBehaviour
    {
        [SerializeField]  bool damageAchievement;
        [SerializeField]  bool killsAchievement;
        [SerializeField]  bool towersPlacedAchievement;

        [SerializeField]  float damageDealt;
        [SerializeField]  int enemiesKilled;
        [SerializeField]  int towersPlaced;
        private void Start()
        {
            damageAchievement = SaveManager.Instance.data.achievements.damage;
            killsAchievement = SaveManager.Instance.data.achievements.kills;
            towersPlacedAchievement = SaveManager.Instance.data.achievements.placed;
            
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
            if (towersPlacedAchievement)
                return;
            
            towersPlaced++;
            if (towersPlaced > 15)
            {
                towersPlacedAchievement = true;
                SaveManager.Instance.data.achievements.placed = true;
                SaveManager.SaveState();
                Debug.Log("Unlocked towers placed achievement (place 15 towers)");
            }
        }

        void OnEnemyKilled(Targetable target, Tower attacker)
        {
            if (killsAchievement)
                return;
            
            enemiesKilled++;
            if (enemiesKilled > 200)
            {
                killsAchievement = true;
                SaveManager.Instance.data.achievements.kills = true;
                SaveManager.SaveState();
                Debug.Log("Unlocked kills achievement (kill 200 enemies)");
            }
        }

        void OnDamageDealt(Targetable targetable, float damage, Tower attacker)
        {
            if (damageAchievement)
                return;
            
            damageDealt += damage;
            if (damageDealt > 10000)
            {
                damageAchievement = true;
                SaveManager.Instance.data.achievements.damage = true;
                SaveManager.SaveState();
                Debug.Log("Unlocked damage achievement (deal 10000 damage)");
            }
        }
    }
}
