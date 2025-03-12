using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class TowerUIButtons : MonoBehaviour
    {
        [SerializeField, ColorPalette] Color upgradable;
        [SerializeField, ColorPalette] Color notUpgradable;

        [SerializeField] Button upgradeButton;
        [SerializeField] Image upgradeOutline;
        [SerializeField] TMP_Text upgradeText;

        [SerializeField] Image sellButton;
        [SerializeField] TMP_Text sellText;

        private Tower tower;

        private void OnEnable()
        {
            GameStats.Instance.coinsChanged += UpdateUI;
        }
        private void OnDisable()
        {
            if(GameStats.Instance)
                GameStats.Instance.coinsChanged -= UpdateUI;
            if(tower)
            {
                tower.upgradeHandler.OnLevelChanged -= UpdateUI;
                tower.stats["upgradeCost"].OnValueChanged -= OnCostChanged;
            }
        }

        public void ShowInfo(Tower tower)
        {
            this.tower = tower;
            gameObject.SetActive(tower != null);
            tower.upgradeHandler.OnLevelChanged += UpdateUI;
            tower.stats["upgradeCost"].OnValueChanged += OnCostChanged;
            UpdateUI();
        }

        private void OnCostChanged(object args) => UpdateUI();
        private void UpdateUI()
        {
            UpdateUpgradeButton();
            sellText.text = $"[{10}] Sell";

        }

        void UpdateUpgradeButton()
        {
            var cost = tower.upgradeHandler.UpgradeCost();
            if(cost == null)
            {
                upgradeOutline.gameObject.SetActive(false);
                return;
            }
            upgradeOutline.gameObject.SetActive(true);

            if(cost < GameStats.Instance.coins)
            {
                upgradeOutline.color = upgradable;
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeOutline.color = notUpgradable;
                upgradeButton.interactable = false;
            }
            upgradeText.text = $"[{cost}] Upgrade";
        }


        public void UpgradeTower()
        {
            var cost = tower.upgradeHandler.UpgradeCost();
            if (cost != null && cost < GameStats.Instance.coins)
            {
                GameStats.Instance.ModifyCoins(-(int)cost);
                tower.upgradeHandler.LevelUp();
            }
            UpdateUI();
        }

        public void SellTower()
        {
            if (!tower) return;

            tower.Tile.RemoveTower();
            Destroy(tower.gameObject);
            GameStats.Instance.ModifyCoins(10);
            UpdateUI();
        }

    }
}
