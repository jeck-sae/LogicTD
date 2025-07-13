using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TowerDefense
{
    public class UpgradeUI : MonoBehaviour
    {
        public TowerUpgradeSeriesSO upgradeSeries;
                
        int currentUpgrade;

        public TMP_Text description;
        public TMP_Text price;
        public Image icon;
        
        
        public void BuyUpgrade()
        {
            if (currentUpgrade >= upgradeSeries.Count)
            {
                Debug.LogWarning("All upgrades have been unlocked");
                return;
            }
            
            var upgrade = upgradeSeries.GetUpgrade(currentUpgrade);

            if (upgrade.cost > GameStats.Instance.coins)
                return;
            
            GameStats.Instance.ModifyCoins(-upgrade.cost);
            
            foreach (var u in upgrade.upgrade)
                GameManager.Instance.UnlockGlobalUpgrade(upgrade.targetTower, u);
            
            currentUpgrade++;
            if(currentUpgrade >= upgradeSeries.Count)
                Destroy(gameObject);
        }
    }
}
