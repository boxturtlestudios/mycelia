using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BoxTurtleStudios.Utilities.Tilemaps;

[CreateAssetMenu(fileName = "New Seeds Item", menuName = "Inventory System/Items/Seeds")] 
public class SeedItemObject : ItemDataObject
{
    public CropData crop;

    private void Awake() 
    {
        type = ItemType.Seeds;    
    }

    public override bool Use(Vector3 position, Grid tileGrid, Tilemap terrain)
    {
        base.Use();
        Transform tilemapTransform = terrain.gameObject.transform;
        for (int i = 0; i < tilemapTransform.childCount; i++)
        {
            if(tilemapTransform.GetChild(i).position == position)
            {
                return tilemapTransform.GetChild(i).GetComponent<FarmSlot>().PlantCrop(crop);
            }
        }
        return false;
    }
}
