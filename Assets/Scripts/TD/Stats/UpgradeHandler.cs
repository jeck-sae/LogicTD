using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UpgradeHandler 
{
    public int CurrentLevel => nextUpgradeIndex + 1;
    public int NextUpgradeIndex => nextUpgradeIndex;
    [SerializeField] int nextUpgradeIndex;

    [SerializeField] List<UpgradeLevel> levels;
    [SerializeField] List<FlagUpgrade> unlockedFlagUpgrades;

    public bool IsMaxLevel => NextUpgradeIndex >= levels.Count;
    protected Stats stats;
    public UpgradeHandler(Stats stats)
    {
        this.stats = stats;
        stats.AddStat("upgradeCost", UpgradeCost() ?? 0, 0);
    }

    public bool UnlockedFlagUpgrade(string flag) => unlockedFlagUpgrades.Any(x => x.flag.Equals(flag));

    public UpgradeLevel GetNextUpgrade()
    {
        if (IsMaxLevel)
            return null;

        return levels[NextUpgradeIndex];
    }

    public List<Stat.StatModifier> GetAllUpgradesForStat(string stat)
    {
        List<Stat.StatModifier> mods = new List<Stat.StatModifier>();
        foreach (var level in levels)
        {
            mods.AddRange(level.statUpgrades.Where(x => x.stat == stat).Select(x => x.modifier));
        }
        return mods;
    }

    public float? UpgradeCost()
    {
        if (levels.Count <= NextUpgradeIndex && levels[NextUpgradeIndex] != null)
            return null;


        return levels[NextUpgradeIndex].cost;
    }

    public void LevelUp()
    {
        if (NextUpgradeIndex >= levels.Count) return;
        nextUpgradeIndex++;

        foreach (var statUpgrade in levels[NextUpgradeIndex].statUpgrades)
        {
            stats.AddModifier(statUpgrade.stat, "upgradeLevel" + CurrentLevel, statUpgrade.modifier.add, statUpgrade.modifier.multiply);
        }
        foreach (var flagUpgrade in levels[NextUpgradeIndex].flagUpgrades)
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