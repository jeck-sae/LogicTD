using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public class DisplayInfoUI : Singleton<DisplayInfoUI>
{
    [SerializeField] GameObject parent;

    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptionText;

    [SerializeField] GameObject shopParent;
    [SerializeField] GameObject statParent;
    [SerializeField] List<StatInfoUI> statList;

    private void Awake()
    {
        Hide();
    }

    public void Show(BaseDisplayInfo info)
    {
        statParent.SetActive(false);
        shopParent.SetActive(false);

        ShowInfo(info);

        if (info is TowerDisplayInfo)
        {
            ShowTower(info as TowerDisplayInfo);
            return;
        }

        if (info is ShopDisplayInfo) 
        {
            ShowShop(info as ShopDisplayInfo);
        }
    }

    protected void ShowShop(ShopDisplayInfo info)
    {
        shopParent?.SetActive(true);
    }

    protected void ShowTower(TowerDisplayInfo info)
    {
        statParent.SetActive(true);
        statList.ForEach(x => x.gameObject.SetActive(false));

        var stats = info.tower.GetStats();
        var upgrades = info.tower.upgradeHandler;

        if (stats != null)
        {
            int i = 0;
            foreach (var stat in stats.stats)
            {
                if (!StatDisplayPreset.HasInfo(stat.Key))
                    continue;

                if (upgrades != null && !upgrades.IsMaxLevel)
                {
                    var upgrade = upgrades.GetNextUpgrade().statUpgrades.FirstOrDefault(x => x.stat == stat.Key);
                    statList[i].DisplayStat(stat.Value, upgrade?.modifier, upgradeHandler: upgrades);
                }
                else
                {
                    statList[i].DisplayStat(stat.Value, upgradeHandler: upgrades);
                }

                i++;
                if (i >= statList.Count)
                    break;
            }
        }
    }

    protected void ShowInfo(BaseDisplayInfo info)
    {
        parent.SetActive(true);
        iconImage.sprite = info.icon;
        titleText.text = info.title;
        descriptionText.text = info.description;
    }

    public void Hide()
    {
        parent.SetActive(false);
    }

}

public class BaseDisplayInfo
{
    public string title;
    public string description;
    public Sprite icon;
    public BaseDisplayInfo() { }
    public BaseDisplayInfo (string title, string description, Sprite icon)
    {
        this.title = title;
        this.description = description;
        this.icon = icon;
    }
}

public class ShopDisplayInfo : BaseDisplayInfo
{
    public ShopDisplayInfo() { }
    public ShopDisplayInfo(string title, string description, Sprite icon) : base(title, description, icon) { }
}

public class TowerDisplayInfo : BaseDisplayInfo 
{
    public Tower tower;

    public TowerDisplayInfo() { }
    public TowerDisplayInfo(string title, string description, Sprite icon, Tower tower) : base(title, description, icon) { this.tower = tower; }
}