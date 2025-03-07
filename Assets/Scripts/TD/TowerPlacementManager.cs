using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacementManager : Singleton<TowerPlacementManager>
{
    [ShowInInspector, ReadOnly]
    public Tower placingTower { get; private set; }
    bool instantiateNew;

    protected Action onPlace;
    protected Action onCancelPlacing;

    protected bool startedPlacingThisFrame;

    public void Update()
    {
        if (placingTower == null)
            return;

        //prevent placing the tower in the same frame it's being selected
        if (startedPlacingThisFrame)
        {
            startedPlacingThisFrame = false;
            return;
        }

        //cancel placing
        if (Input.GetKey(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            CancelPlacing();
            return;
        }

        Tile hoveringTile = GridManager.Instance.GetHoveringTile();

        //place tower
        if (Input.GetMouseButtonDown(0) && hoveringTile && hoveringTile.CanPlace() && !Helpers.IsOverUI)
        {
            PlaceSelectedTower(hoveringTile);
            return;
        }
    }


    void PlaceSelectedTower(Tile tile)
    {
        if (!tile || !placingTower) return;

        InputManager.Instance.SetPlacingStatus(false);

        Tower tower = null;
        if (instantiateNew)
        {
            tower = Instantiate(placingTower.gameObject).GetComponent<Tower>();
        }
        else
        {
            tower = placingTower;
        }

        tower.gameObject.SetActive(true);
        tower.name = placingTower.name;
        tile.PlaceTower(tower);
        onPlace?.Invoke();

        AudioController.Instance.PlaySound2D("ui_confirm");

        StopPlacing();
    }


    public void CancelPlacing()
    {
        onCancelPlacing?.Invoke();
        StopPlacing();
        AudioController.Instance.PlaySound2D("ui_cancel");
    }

    protected void StopPlacing()
    {
        placingTower = null;
        onPlace = null;
        onCancelPlacing = null;

        TowerPreviewManager.Instance.StopPreviewing();

        InputManager.Instance.SetPlacingStatus(false);
    }

    public void StartPlacing(Tower tower, Action onPlace = null, Action cancelPlacing = null, bool instantiateNew = true)
    {
        if (placingTower == tower)
        {
            CancelPlacing();
            return;
        }

        InputManager.Instance.SetPlacingStatus(true);
        this.instantiateNew = instantiateNew;
        this.placingTower = tower;
        this.onPlace = onPlace;
        this.onCancelPlacing = cancelPlacing;
        startedPlacingThisFrame = true;

        TowerPreviewManager.Instance.PreviewTower(tower, true);

        //Hide tower
        if (!instantiateNew) 
        { 
            tower.transform.position = Vector3.left * 10000;
            tower.Tile.RemoveTower();
        }

        AudioController.Instance.PlaySound2D("ui_confirm");
    }
}