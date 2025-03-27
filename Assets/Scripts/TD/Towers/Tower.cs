using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Drawing;


namespace TowerDefense
{
    public class Tower : Interactable2D, IStatObject
    {
        public Tile Tile { get; set; }
    
        [HideInInspector] public EffectHandler effects;
    
        [Header("General")]
        public Sprite shopIcon;
        public string towerName;
        [TextArea] public string towerDescription;
        [ColorPalette] public UnityEngine.Color towerColor;
    
        public Stat Cost;
        public Stat MaxRange;
        public Stat MinRange;
    
        public Stats stats;
        public UpgradeHandler upgradeHandler;
    
        [BoxGroup("Sound"), Range(0f, 1f)]
        public float placeSoundVolume = .5f;
    
        protected RangeIndicator minRangeIndicator;
        protected RangeIndicator maxRangeIndicator;

        protected bool isSelected;

        protected override void ManagedInitialize()
        {
            if(!effects)
                effects = gameObject.AddComponent<EffectHandler>();
    
            stats = GetStats();
            upgradeHandler.SetTower(this);
    
            SetupRangeIndicators();


            AudioController.Instance.PlaySound2D("tower_" + towerName + "_place", placeSoundVolume);
        }

        protected override void Start()
        {
            base.Start();
            if (TileSelectionManager.Instance?.SelectedTile == Tile)
                OnTileSelected();
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
            maxRangeIndicator = Instantiate(Resources.Load("Prefabs/UI/RangePreview"), transform).GetComponent<RangeIndicator>();
            maxRangeIndicator.scaleWithStat.multiply = 2;
            maxRangeIndicator.scaleWithStat.SetStat(MaxRange);
            maxRangeIndicator.Hide();
                
            minRangeIndicator = Instantiate(Resources.Load("Prefabs/UI/RangePreview"), transform).GetComponent<RangeIndicator>();
            minRangeIndicator.scaleWithStat.multiply = 2;
            minRangeIndicator.scaleWithStat.SetStat(MinRange);
            minRangeIndicator.Hide();
        }
    
        protected override void OnCursorEnter()
        {
            if (!isSelected)
            {
                maxRangeIndicator.ShowColor(new UnityEngine.Color(1, 1, 1, 0.3f));
                minRangeIndicator.ShowColor(new UnityEngine.Color(1, 0, 0, 0.3f));
            }
        }

        protected override void OnCursorExit()
        {
            if (!isSelected)
            {
                maxRangeIndicator.Hide();
                minRangeIndicator.Hide();
            }
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
    

        public void OnTileSelected()
        {
            isSelected = true;
            maxRangeIndicator.ShowColor(towerColor);
            minRangeIndicator.ShowColor(UnityEngine.Color.red);
        }
    
        public void OnTileDeselected()
        {
            isSelected = false;
            maxRangeIndicator.Hide();
            minRangeIndicator.Hide();
        }

        public virtual BaseDisplayInfo GetDisplayInfo()
        {
            return new TowerDisplayInfo(towerName, towerDescription, shopIcon, this);
        }
    
    
        public void DestroyTower()
        {
            Tile.RemoveTower();
            Destroy(gameObject);
        }
    
        public void StartMoving()
        {
            Tile.RemoveTower();
            TowerPlacementManager.Instance.StartPlacing(this, OnMoveAway, OnCancelMoving, false);
        }
    
        protected void OnMoveAway()
        {
            isSelected = TileSelectionManager.Instance?.SelectedTile == Tile;
        }
    
        protected void OnCancelMoving()
        {
            isSelected = TileSelectionManager.Instance?.SelectedTile == Tile;
            gameObject.SetActive(true);
            this.Tile.PlaceTower(this);
        }
    }
}
