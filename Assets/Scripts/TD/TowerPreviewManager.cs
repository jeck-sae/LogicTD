using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class TowerPreviewManager : Singleton<TowerPreviewManager>
{
    [ShowInInspector, ReadOnly]
    public Tower previewingTower { get; private set; }
    Vector2Int startPosition;
    Func<bool> placeCondition;

    bool usingOriginalGFX;
    bool followCursor;
    bool isPreviewing;

    protected GameObject preview;
    protected SpriteRenderer[] previewRenderers;
    protected RangeIndicator rangePreview;

    private void Awake()
    {
        rangePreview = Instantiate(Resources.Load("Prefabs/UI/RangePreview"), transform).GetComponent<RangeIndicator>();
        rangePreview.scaleWithStat.multiply = 2;
        rangePreview.gameObject.SetActive(false);
    }


    public void Update()
    {
        if (!isPreviewing) 
            return;

        UpdatePreview();
    }

    void UpdatePreview()
    {
        Vector2Int coords = followCursor ? GridManager.FixCoordinates(Helpers.Camera.ScreenToWorldPoint(Input.mousePosition))
                                         : startPosition;

        var tile = GridManager.Instance.Get(coords);

        if (followCursor)
        {
            rangePreview.transform.position = coords.ToVector3();
            preview.transform.position = coords.ToVector3();
        }

        bool conditionMet = (placeCondition == null) ? true : placeCondition();
        if (tile && tile.CanPlace() && conditionMet)
        {
            previewRenderers.ForEach(x => x.color = Color.white);
            rangePreview.SetColor(previewingTower.towerColor);
        }
        else
        {
            previewRenderers.ForEach(x => x.color = Color.red);
            rangePreview.SetColor(Color.red);
        }
    }

    public void StopPreviewing()
    {
        isPreviewing = false;

        if (preview)
        {
            if(usingOriginalGFX)
                preview.transform.localPosition = Vector3.zero;
            else
                Destroy(preview);
        }

        rangePreview.gameObject.SetActive(false);
    }

    public void PreviewTower(Tower tower, bool useOriginalGFX = false, Func<bool> placeCondition = null)
    {
        var pos = GridManager.FixCoordinates(Helpers.Camera.ScreenToWorldPoint(Input.mousePosition));
        PreviewTower(tower, pos, true, useOriginalGFX, placeCondition);
    }

    public void PreviewTower(Tower tower, Vector2Int position, bool useOriginalGFX = false, Func<bool> placeCondition = null)
        => PreviewTower(tower, position, false, useOriginalGFX, placeCondition);

    private void PreviewTower(Tower tower, Vector2Int position, bool followCursor, bool useOriginalGFX, Func<bool> placeCondition)
    {
        if (isPreviewing)
            StopPreviewing();

        this.previewingTower = tower;
        this.followCursor = followCursor;
        this.startPosition = position;
        this.usingOriginalGFX = useOriginalGFX;
        this.placeCondition = placeCondition;
        this.isPreviewing = true;

        //Range preview
        var pos = new Vector3(position.x, position.y, rangePreview.transform.position.z);
        rangePreview.transform.position = pos;
        rangePreview.scaleWithStat.SetStat(tower.MaxRange);
        rangePreview.gameObject.SetActive(true);
        rangePreview.SetColor(tower.towerColor);

        //Tower preview
        var gfx = tower.transform.Find("GFX");
        if (useOriginalGFX)
            preview = gfx.gameObject;
        else
            preview = Instantiate(gfx, pos, Quaternion.identity).gameObject;

        previewRenderers = preview.GetComponentsInChildren<SpriteRenderer>();
    }


}
