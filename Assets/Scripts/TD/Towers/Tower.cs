using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System.Collections.Generic;

public class Tower : Interactable2D, IStatObject
{
    public Tile Tile { get; set; }

    [HideInInspector] public EffectHandler effects;

    public Sprite shopIcon;
    [Header("General")]
    public string towerName;
    [TextArea]
    public string towerDescription;
    public Color towerColor;

    public Stat Cost;
    public Stat MaxRange;
    public Stat MinRange;

    public Stats stats;
    public UpgradeHandler upgradeHandler;

    [BoxGroup("Sound"), Range(0f, 1f)]
    public float placeSoundVolume = .5f;

    protected ScaleWithStat minRangeIndicator;
    protected ScaleWithStat maxRangeIndicator;
    protected bool placedThisFrame = true;

    protected override void ManagedInitialize()
    {
        if(!effects)
            effects = gameObject.AddComponent<EffectHandler>();

        stats = GetStats();

        SetupRangeIndicators();

        AudioController.Instance.PlaySound2D("tower_" + towerName + "_place", placeSoundVolume);

    }

    private void OnEnable()
    {
        placedThisFrame = true;
        CustomCoroutine.WaitThenExecute(0, () => { placedThisFrame = false; }, true);
    }

    public virtual Stats GetStats()
    {
        if (stats != null)
            return stats;

        var tempStats = new Stats();
        tempStats.AddStat("cost", Cost);
        tempStats.AddStat("maxRange", MaxRange);
        tempStats.AddStat("minRange", MinRange);
        return tempStats;
    }
        
    protected void SetupRangeIndicators()
    {
        maxRangeIndicator = Instantiate(Resources.Load("Prefabs/RangePreview"), transform).GetComponent<ScaleWithStat>();
        maxRangeIndicator.multiply = 2;
        maxRangeIndicator.SetStat(MaxRange);
        maxRangeIndicator.GetComponent<RangeIndicator>().SetColor(towerColor);
        maxRangeIndicator.gameObject.SetActive(false);
            
        minRangeIndicator = Instantiate(Resources.Load("Prefabs/RangePreview"), transform).GetComponent<ScaleWithStat>();
        minRangeIndicator.multiply = 2;
        minRangeIndicator.SetStat(MinRange);
        minRangeIndicator.GetComponent<RangeIndicator>().SetColor(Color.red);
        minRangeIndicator.gameObject.SetActive(false);
    }

    protected override void OnCursorEnter()
    {
        maxRangeIndicator.gameObject.SetActive(true);
        minRangeIndicator.gameObject.SetActive(true);
    }

    protected override void OnCursorExit()
    {
        maxRangeIndicator.gameObject.SetActive(false);
        minRangeIndicator.gameObject.SetActive(false);
    }

    public virtual BaseDisplayInfo GetDisplayInfo()
    {
        return new TowerDisplayInfo(towerName, towerDescription, shopIcon, this);
    }

    protected override void OnCursorSelectStart()
    {
        //DisplayInfoUI.Instance.Show(this, shopIcon, towerName, towerDescription, true, stats, upgradeHandler);


        /*if (!InputManager.Instance.acceptInput || placedThisFrame)
            return;

        InputManager.Instance.SetMovingStatus(true);
        this.Tile.tower = null; //hacky way of doing it. Works for now
        TowerPlacementManager.Instance.StartPlacing(this, OnMoveAway, OnCancelMoving, false);*/
    }


    public void StartMoving()
    {
        InputManager.Instance.SetMovingStatus(true);
        Tile.RemoveTower();
        TowerPlacementManager.Instance.StartPlacing(this, OnMoveAway, OnCancelMoving, false);
    }

    protected void OnMoveAway()
    {
        //Destroy(gameObject);
        InputManager.Instance.SetMovingStatus(false);
    }

    protected void OnCancelMoving()
    {
        gameObject.SetActive(true);
        this.Tile.PlaceTower(this);
        InputManager.Instance.SetMovingStatus(false);
    }
}