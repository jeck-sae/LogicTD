using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class GameEvents : MonoBehaviour
    {
        //Game Over
        public static event Action OnGameOver;
        public static void GameOver() 
            => OnGameOver?.Invoke();
        
        
        //Damage Dealt
        public static event Action<Targetable, float, Tower> OnDamageDealt;
        public static void DamageDealt(Targetable target, float damageDealt, Tower attacker)
            => OnDamageDealt?.Invoke(target, damageDealt, attacker);
        
        //Tower Played
        public static event Action<Tower> OnTowerPlayed;
        public static void TowerPlayed(Tower tower)
            => OnTowerPlayed?.Invoke(tower);
        
        //Enemy Killed
        public static event Action<Targetable, Tower> OnEnemyKilled;
        public static void EnemyKilled(Targetable target, Tower attacker)
            => OnEnemyKilled?.Invoke(target, attacker);
    }
}
