using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public GameObject prefab;
    public Image iconUI;
    public TMP_Text costUI;

    protected Tower tower;
    protected void Awake()
    {
        tower = prefab.GetComponent<Tower>();
        iconUI.sprite = tower.shopIcon;
        costUI.text = ((int)tower.Cost).ToString();
        GameStats.Instance.coinsChanged += UpdatePriceColor;
        UpdatePriceColor();
    }

    public void Select()
    {
        //If already placing, deselect instead
        if (TowerPlacementManager.Instance.placingTower == tower)
        {
            TowerPlacementManager.Instance.CancelPlacing();
            return;
        }
        //Check input is not occupied (for example playing a potion or a different tower)
        if (InputManager.Instance && !InputManager.Instance.acceptInput)
            return;
        //Check player has enough money
        if (GameStats.Instance && GameStats.Instance.coins < tower.Cost)
            return;

        //TOGGLE SELECT
        TowerPlacementManager.Instance.StartPlacing(tower, OnTowerPlaced);

        if (TowerPlacementManager.Instance.placingTower)
            AudioController.Instance.PlaySound2D("ui_confirm");
        else
            AudioController.Instance.PlaySound2D("ui_cancel");
    }

    public void OnCursorHover()
    {
        DisplayInfoUI.Instance.Show(this, tower.shopIcon, tower.towerName, tower.towerDescription, false, tower.GetStats());
    }
    public void OnCursorExit()
    {
        DisplayInfoUI.Instance.Hide(this);
    }

    protected void OnTowerPlaced()
    {
        GameStats.Instance?.ModifyCoins(-(int)tower.Cost);
    }

    protected void UpdatePriceColor()
    {
        if (tower.Cost <= GameStats.Instance.coins)
            costUI.color = TDColors.AffordableColor;
        else
            costUI.color = TDColors.UnaffordableColor;

    }
}
