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

    public override bool Use(Vector3 position, Grid tileGrid, Tilemap terrain)
    {
        base.Use();
        switch(toolType)
        {
            case ToolType.Shovel:
                if(TilemapUtilities.FindCurrentTile<RuleTile>(position, tileGrid, terrain) == grassTile)
                {
                    playerObj.GetComponent<Animator>().SetTrigger("Dig");
                    TilemapUtilities.SetCurrentTile(dirtTile, position, tileGrid, terrain);
                    return true;
                }
            break;

            
            case ToolType.Hoe:
                if(TilemapUtilities.FindCurrentTile<TileBase>(position, tileGrid, terrain) == dirtTile)
                {
                    TilemapUtilities.SetCurrentTile(farmTile, position, tileGrid, terrain);
                    return true;
                }
            break;

            case ToolType.WateringCan:
                Transform tilemapTransform = terrain.gameObject.transform;
                for (int i = 0; i < tilemapTransform.childCount; i++)
                {
                    if(tilemapTransform.GetChild(i).position == position)
                    {
                        tilemapTransform.GetChild(i).GetComponent<FarmSlot>().WaterCrop();
                        return true;
                    }
                }
            break;

        }
        return false;
    }
}
