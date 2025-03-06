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

    protected GameObject preview;
    protected SpriteRenderer[] previewRenderers;

    protected RangeIndicator rangePreview;
    protected bool startedPlacingThisFrame;

    private void Awake()
    {
        rangePreview = Instantiate(Resources.Load("Prefabs/UI/RangePreview"), transform).GetComponent<RangeIndicator>();
        rangePreview.scaleWithStat.multiply = 2;
        rangePreview.gameObject.SetActive(false);
    }


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
        if (Input.GetMouseButtonDown(0) && hoveringTile && hoveringTile.CanPlace())
        {
            PlaceSelectedTower(hoveringTile);
            return;
        }


        //draw preview
        UpdatePreview();
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
            preview.transform.parent = tower.transform;
            preview.transform.localPosition = Vector3.zero;
            preview = null;
        }
        previewRenderers = new SpriteRenderer[0];

        tower.gameObject.SetActive(true);
        tower.name = placingTower.name;
        tile.PlaceTower(tower);
        onPlace?.Invoke();

        AudioController.Instance.PlaySound2D("ui_confirm");

        StopPlacing();
    }

    void UpdatePreview()
    {
        Vector3 coords = Vector3.zero;
        var tile = GridManager.Instance.GetHoveringTile();
        if (!tile) coords = GridManager.FixCoordinates(Helpers.Camera.ScreenToWorldPoint(Input.mousePosition)).ToVector3();
        else coords = tile.Position.ToVector3();

        rangePreview.transform.position = coords;
        preview.transform.position = coords;

        if (tile && tile.CanPlace())
        {
            previewRenderers.ForEach(x => x.color = Color.white);
        }
        else
        {
            previewRenderers.ForEach(x => x.color = Color.red);
        }
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

        if(preview)
            Destroy(preview);
        
        rangePreview.gameObject.SetActive(false);
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

        //Range preview
        var hoveringCoords = GridManager.FixCoordinates(Input.mousePosition);
        rangePreview.transform.position = new Vector3(hoveringCoords.x, hoveringCoords.y, rangePreview.transform.position.z);
        rangePreview.scaleWithStat.SetStat(tower.MaxRange);
        rangePreview.gameObject.SetActive(true);
        rangePreview.SetColor(tower.towerColor);

        //Tower preview
        var pos = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition);
        if (instantiateNew)
            preview = Instantiate(tower.transform.Find("GFX"), pos, Quaternion.identity).gameObject;
        else
        {
            preview = tower.transform.Find("GFX").gameObject;
            tower.transform.position = Vector3.left * 10000;
            tower.Tile.RemoveTower();
        }
        previewRenderers = preview.GetComponentsInChildren<SpriteRenderer>();

        AudioController.Instance.PlaySound2D("ui_confirm");
    }
}