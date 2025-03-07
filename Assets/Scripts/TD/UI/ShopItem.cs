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
        BuyTower();
        /*
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
            AudioController.Instance.PlaySound2D("ui_cancel");*/
    }

    public void OnCursorHover()
    {
        if (TileSelectionManager.Instance.SelectedTile)
            TowerPreviewManager.Instance.PreviewTower(tower, TileSelectionManager.Instance.SelectedTile.Position, placeCondition: () => { return GameStats.Instance.coins >= tower.Cost; });
        //DisplayInfoUI.Instance.Show(this, tower.shopIcon, tower.towerName, tower.towerDescription, false, tower.GetStats(), tower.upgradeHandler);
    }
    public void OnCursorExit()
    {
        TowerPreviewManager.Instance.StopPreviewing();
        //DisplayInfoUI.Instance.Hide(this);
    }

    protected void OnTowerPlaced()
    {
        GameStats.Instance?.ModifyCoins(-(int)tower.Cost);
    }
    
    protected void BuyTower()
    {
        if (GameStats.Instance && GameStats.Instance.coins < tower.Cost)
            return;

        if (TileSelectionManager.Instance.SelectedTile && TileSelectionManager.Instance.SelectedTile.Tower == null)
        {
            GameStats.Instance.ModifyCoins(-(int)tower.Cost);
            
            var t = Instantiate(prefab).GetComponent<Tower>();
            TileSelectionManager.Instance.SelectedTile.PlaceTower(t);
            
            TowerPreviewManager.Instance.StopPreviewing();
            DisplayInfoUI.Instance.Show(TileSelectionManager.Instance.SelectedTile.GetDisplayInfo());
        }
    }

    protected void UpdatePriceColor()
    {
        if (tower.Cost <= GameStats.Instance.coins)
            costUI.color = TDColors.AffordableColor;
        else
            costUI.color = TDColors.UnaffordableColor;
    }
}
