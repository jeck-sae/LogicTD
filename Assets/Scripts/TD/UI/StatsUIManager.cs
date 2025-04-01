using UnityEngine;
using TMPro;


namespace TowerDefense
{
    public class StatsUIManager : MonoBehaviour
    {
        public TMP_Text lives;
        public TMP_Text coins;
        public TMP_Text waves;
    
        private void Awake()
        {
            GameStats.Instance.livesChanged += UpdateLives;
            GameStats.Instance.coinsChanged += UpdateCoins;
            WaveManager.Instance.SpawningNewWave += UpdateWave;
        }
    
        private void Start()
        {
            UpdateLives();
            UpdateCoins();
            UpdateWave(0);
        }
    
        protected void UpdateWave(int wave)
        {
            if(GameManager.Instance.winGameAtWave == -1)
                waves.text = (wave + 1).ToString();
            else
                waves.text = (wave + 1).ToString() + "/" + GameManager.Instance.winGameAtWave;
        }
        protected void UpdateLives()
        {
            lives.text = GameStats.Instance.lives.ToString();
        }
        protected void UpdateCoins()
        {
            coins.text = GameStats.Instance.coins.ToString();
        }
    }
    
}
