using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake() 
    {
        type = ItemType.Tool;
    }
}
