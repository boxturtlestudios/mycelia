using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Inventory System/Items/Consumable")]
public class ConsumableItemObject : ItemDataObject
{
    public int energyChange;

    private void Awake() 
    {
        type = ItemType.Consumable;
    }
}
    
