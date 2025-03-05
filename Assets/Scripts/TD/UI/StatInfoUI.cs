using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatInfoUI : MonoBehaviour
{
    [SerializeField] TMP_Text statName;
    [SerializeField] TMP_Text statValue;
    [SerializeField] Image currentValue;
    [SerializeField] Image upgradeValue;

    Stat stat;
    Stat.StatModifier modifier;
    float maxValue;
    public void DisplayStat(Stat stat, Stat.StatModifier previewModifier = null, UpgradeHandler upgradeHandler = null)
    {
        this.maxValue = Stat.GetValueWithModifiers(stat, upgradeHandler.GetAllUpgradesForStat(stat.Name));
        if (maxValue <= 0)
            return;

        this.stat = stat;
        this.modifier = previewModifier;
        stat.OnValueChanged += UpdateValue;
        gameObject.SetActive(true);
        UpdateValue(null);
    }

    public void UpdateValue(object args)
    {

        statName.text = StatDisplayPreset.GetInfo(stat.Name).name;
        statValue.text = stat.Value.ToString();

        float currentProgress = stat / maxValue;


        currentValue.transform.localScale = new Vector3(currentProgress, 1, 1);
        currentValue.color = StatDisplayPreset.GetInfo(stat.Name).color;

        if (modifier != null)
        {
            var mods = stat.modifiers.Values.ToList();
            mods.Add(modifier);
            float upgradedValue = Stat.GetValueWithModifiers(stat, mods);
            float upgradeProgress = upgradedValue / maxValue;
            
            upgradeValue.transform.localScale = new Vector3 (upgradeProgress, 1, 1);
        }
        else
        {
            upgradeValue.transform.localScale = Vector3.zero;
        }
    }
}
