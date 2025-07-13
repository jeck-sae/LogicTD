using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu(menuName = "Tower Defense/Upgrade Series", fileName = "UpgradeSeries")]
    public class TowerUpgradeSeriesSO : SerializedScriptableObject
    {
        public string defaultTargetTower;
        public string defaultTargetStat;
        [TextArea] public string defaultDescription;
        [NonSerialized, OdinSerialize] public TowerUpgrade[] upgrades;

        
        public int Count => upgrades.Length;

        public TowerUpgrade GetUpgrade(int index)
        {
            if (index >= upgrades.Length || index < 0)
            {
                Debug.LogError($"Upgrade index out of range: {index}/{upgrades.Length - 1}", this);
                return null;
            }
            
            var u = upgrades[index];
            
            if(u.targetTower.Trim() == "_")
                u.targetTower = defaultTargetTower;
            if(u.description.Trim() == "_")
                u.description = defaultDescription;
            
            return u;
        }
    }
}