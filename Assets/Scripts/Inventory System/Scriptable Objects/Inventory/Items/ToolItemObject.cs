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
    public int damage;
    [Header("Tiles")]
    public Tile dirtTile;
    public Tile farmTile;
    public RuleTile grassTile;

    [Header("Misc")]
    public LayerMask axeLayer;
    public LayerMask pickLayer;

    private Vector3 selectedPosTEMP = Vector3.zero;

    private void Awake() 
    {
        type = ItemType.Tool;
    }

    public override bool Use(Vector3 position, Grid tileGrid, Tilemap terrain, out string trigger)
    {
        selectedPosTEMP = position;

        trigger = null;
        base.Use();
        switch(toolType)
        {
            case ToolType.Shovel:
                trigger = "Dig";

                if(TilemapUtilities.FindCurrentTile<RuleTile>(position, tileGrid, terrain) == grassTile)
                {
                    TilemapUtilities.SetCurrentTile(dirtTile, position, tileGrid, terrain);
                    return true;
                }
            break;

            
            case ToolType.Hoe:
                trigger = "Hoe";
                SoundManager.Instance.Play("Swing");

                if(TilemapUtilities.FindCurrentTile<TileBase>(position, tileGrid, terrain) == dirtTile)
                {
                    Transform tilemapTrans = terrain.gameObject.transform;
                    for (int i = 0; i < tilemapTrans.childCount; i++)
                    {
                        if(Vector2.Distance(tilemapTrans.GetChild(i).position, position) < 0.5f)
                        {
                            if(tilemapTrans.GetChild(i).CompareTag("Object"))
                            {
                                Debug.Log("Object obstructing tile");
                                return false;
                            }
                        }
                    }
                    TilemapUtilities.SetCurrentTile(farmTile, position, tileGrid, terrain);
                    return true;
                }
            break;

            case ToolType.Axe:
                trigger = "Axe";
                SoundManager.Instance.Play("Swing");

                Collider2D[] axeHits = Physics2D.OverlapCircleAll(position, 0.4f, axeLayer);
                foreach(Collider2D hit in axeHits)
                {
                    //Only test for the axe collider
                    if(!hit.isTrigger) { continue; }

                    if(hit.gameObject.GetComponent<ToolHittable>() != null)
                    {
                        hit.gameObject.GetComponent<ToolHittable>().Hit(damage, 0.3f);
                        return true;
                    }
                }
            break;

            case ToolType.Pickaxe:
                trigger = "Pickaxe";
                SoundManager.Instance.Play("Swing");

                Transform tilemap = terrain.gameObject.transform;
                for (int i = 0; i < tilemap.childCount; i++)
                {
                    if(Vector2.Distance(tilemap.GetChild(i).position, position) < 0.5f)
                    {
                        if(tilemap.GetChild(i).gameObject.layer != LayerMask.NameToLayer("Pickaxe")) { return false; }
                        if(tilemap.GetChild(i).GetComponent<ToolHittable>())
                        {
                            tilemap.GetChild(i).GetComponent<ToolHittable>().Hit(damage, 0.3f);
                            return true;
                        }
                    }
                }

                // Collider2D[] pickHits = Physics2D.OverlapCircleAll(position, 0.25f, pickLayer);
                // foreach(Collider2D hit in pickHits)
                // {
                //     if(Vector2.Distance(hit.transform.position, position) < 0.4f)
                //     {
                //         //Only test for the pick collider
                //         if(!hit.isTrigger) { continue; }

                //         if(hit.gameObject.GetComponent<ToolHittable>() != null)
                //         {
                //             hit.gameObject.GetComponent<ToolHittable>().Hit(damage, 0.3f);
                //             return true;
                //         }
                //     }
                // }

            break;

            case ToolType.WateringCan:
                trigger = "Water";

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
