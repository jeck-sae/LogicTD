using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UpgradeHandler 
{
    public int CurrentLevel => currentLevel + 1;
    public int CurrentLevelIndex => currentLevel;
    [SerializeField] int currentLevel;

    [SerializeField] List<UpgradeLevel> levels;
    [SerializeField] List<FlagUpgrade> unlockedFlagUpgrades;

    protected Stats stats;
    public UpgradeHandler(Stats stats)
    {
        this.stats = stats;
        stats.AddStat("upgradeCost", UpgradeCost() ?? 0, 0);
    }

    public bool UnlockedFlagUpgrade(string flag) => unlockedFlagUpgrades.Any(x => x.flag.Equals(flag));

    public float? UpgradeCost()
    {
        if (levels.Count <= CurrentLevelIndex && levels[CurrentLevelIndex] != null)
            return null;


        return levels[CurrentLevelIndex].cost;
    }

    public void LevelUp()
    {
        if (CurrentLevelIndex >= levels.Count) return;
        currentLevel++;

        foreach (var statUpgrade in levels[CurrentLevelIndex].statUpgrades)
        {
            stats.AddModifier(statUpgrade.stat, "upgradeLevel" + CurrentLevel, statUpgrade.modifier.add, statUpgrade.modifier.multiply);
        }
        foreach (var flagUpgrade in levels[CurrentLevelIndex].flagUpgrades)
        {
            unlockedFlagUpgrades.Add(flagUpgrade);
        }
    }





    [Serializable]
    public class UpgradeLevel
    {
        public float cost;
        public List<StatUpgrade> statUpgrades;
        public List<FlagUpgrade> flagUpgrades;
    }

    [Serializable]
    public class StatUpgrade
    {
        public string stat;
        public Stat.StatModifier modifier;
    }

    [Serializable]
    public class FlagUpgrade
    {
        public string flag;
        public string description;
}
}