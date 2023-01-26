using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Generic Item", menuName = "Inventory System/Items/Generic", order = 1)]
public class GenericItemObject : ItemDataObject
{
    private void Awake() 
    {
        type = ItemType.Generic;
    }
}
