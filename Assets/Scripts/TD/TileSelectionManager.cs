using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class TileSelectionManager : Singleton<TileSelectionManager>
{
    public Tile SelectedTile => selectedTile;
    [ShowInInspector, ReadOnly] Tile selectedTile;

    [SerializeField] Transform selectionIndicator;

    private Action<Tile> specialSelectAction;
    private bool selectTileAfterSpecialAction;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            Tile hovering = GridManager.Instance.GetHoveringTile();

            if(!hovering || hovering == selectedTile)
            {
                if(!Helpers.IsOverUI)
                    DeselectTile();
            }
            else
                SelectTile(hovering);
        }

        if (Input.GetMouseButtonDown(1) && selectedTile != null)
            DeselectTile();
    }

    void DeselectTile()
    {
        selectedTile = null;
        selectionIndicator.gameObject.SetActive(false);
        DisplayInfoUI.Instance.Hide();
    }

    void SelectTile(Tile tile)
    {
        if(specialSelectAction != null)
        {
            specialSelectAction(tile);
            specialSelectAction = null;

            if (!selectTileAfterSpecialAction)
                return;
        }
        
        selectionIndicator.gameObject.SetActive(true);
        selectionIndicator.position = tile.transform.position;

        DisplayInfoUI.Instance.Show(tile.GetDisplayInfo());

        selectedTile = tile;
    }


    public void SetSpecialSelectAction(Action<Tile> specialSelectAction, bool selectAfterAction)
    {
        this.specialSelectAction = specialSelectAction;
        this.selectTileAfterSpecialAction = selectAfterAction;
    }

}
