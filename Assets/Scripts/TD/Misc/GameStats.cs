using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TowerDefense
{
    public class GameStats : Singleton<GameStats>
    {
        public int lives;
        public int coins;
        public int coinsPerSecond;
        public event Action livesChanged;
        public event Action coinsChanged;

        private float moneyTimer;
        private void Update()
        {
            if (!WaveManager.Instance.SpawningPaused)
            {
                moneyTimer += Time.deltaTime;
                if (moneyTimer >= 1)
                {
                    moneyTimer -= 1;
                    ModifyCoins(coinsPerSecond);
                }
            }
        }

        public void LoseHP(int amount)
        {
            lives -= amount;
            livesChanged?.Invoke();
        }
        
        public void ModifyCoins(int amount)
        {
            coins += amount;
            coinsChanged?.Invoke();
        }

        public void SetCoinsPerSecond(int amount)
        {
            coinsPerSecond = amount;
        }
    }
}
