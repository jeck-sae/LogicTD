using UnityEngine;
using UnityEngine.Serialization;

namespace TowerDefense
{
    public class UpgradeUI : MonoBehaviour
    {
        public TowerUpgradeSeriesSO upgradeSeries;
                
        int currentUpgrade;
        
        
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
        }
    }
}
