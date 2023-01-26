using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Utilities/Item Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemDataObject[] Items;
    public Dictionary<int, ItemDataObject> GetItem = new Dictionary<int, ItemDataObject>();

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemDataObject>();
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            GetItem.Add(i, Items[i]);
            Items[i].id = i;
        }
    }
}
