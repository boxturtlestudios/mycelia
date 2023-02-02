using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoxTurtleStudios.Utilities.Tilemaps;
using UnityEngine.Tilemaps;

public enum ToolType
{
    Shovel,
    Hoe,
    Axe,
    Pickaxe,
    Sickle,
    WateringCan,
    FishingRod
}

[CreateAssetMenu(fileName = "New Tool Item", menuName = "Inventory System/Items/Tool")]   
public class ToolItemObject : ItemDataObject
{
    public ToolType toolType;
    [Header("Tiles")]
    public Tile dirtTile;
    public Tile farmTile;
    public RuleTile grassTile;

    private void Awake() 
    {
        type = ItemType.Tool;
    }

    public override void Use(Vector3 position, Grid tileGrid, Tilemap terrain)
    {
        base.Use();
        switch(toolType)
        {
            case ToolType.Shovel:
                if(TilemapUtilities.FindCurrentRuleTile(position, tileGrid, terrain) == grassTile)
                {
                    TilemapUtilities.SetCurrentTile(dirtTile, position, tileGrid, terrain);
                }
            break;

            case ToolType.Hoe:
                if(TilemapUtilities.FindCurrentTile(position, tileGrid, terrain) == dirtTile)
                {
                    TilemapUtilities.SetCurrentTile(farmTile, position, tileGrid, terrain);
                }
            break;

        }
    }
}
