using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class DisplayInfoUI : Singleton<DisplayInfoUI>
{
    [SerializeField] GameObject parent;

    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptionText;

    [SerializeField] List<StatInfoUI> statList;

    private void Awake()
    {
        Hide(null);
    }

    object shownBy;
    public void Show(object shownBy, Sprite icon, string title, string description, bool toggle = false, Stats displayStats = null, UpgradeHandler upgrades = null)
    {
        if(toggle && this.shownBy == shownBy)
        {
            Hide(shownBy);
            return;
        }

        parent.SetActive(true);
        iconImage.sprite = icon;
        titleText.text = title;
        descriptionText.text = description;
        this.shownBy = shownBy;

        statList.ForEach(x => x.gameObject.SetActive(false));

        if (displayStats != null)
        {
            int i = 0;
            foreach(var stat in displayStats.stats)
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

    public void Hide(object hideInfoShownBy)
    {
        if (shownBy != hideInfoShownBy)
            return;

        ForceHide();
    }
    public void ForceHide()
    {
        shownBy = null;
        parent.SetActive(false);
    }
}