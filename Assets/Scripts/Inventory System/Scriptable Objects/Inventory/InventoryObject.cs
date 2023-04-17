using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    [Tooltip("System path to save inventory to")]
    public string savePath;
    private ItemDatabaseObject database;

    public delegate void UpdateInventoryDisplay();
    public static event UpdateInventoryDisplay OnUpdate;

    public int maxSlots;
    //public List<InventorySlot> container = new List<InventorySlot>();
    public InventorySlot[] container;

    private void OnEnable() 
    {
        /*for (int i = 0; i < container.Length; i++)
        {
            container[i] = new InventorySlot();
        }*/

        InventorySlot.OnUpdateInventory += UpdateInventory;

        #if UNITY_EDITOR
            database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Item Database.asset", typeof(ItemDatabaseObject));
        #else
            database = Resources.Load<ItemDatabaseObject>("Item Database");
        #endif
    }
    private void OnDisable() 
    {
        InventorySlot.OnUpdateInventory -= UpdateInventory;
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < container.Length; i++)
        {
            if (container[i].count <= 0)
            {
                container[i] = new InventorySlot();
            }
        }

        //Call update inventory display event
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

    public void AddItem(ItemDataObject _item, int _amount)
    {
        bool hasItem = false;
        if (_item.stackable)
        {
            for (int i = 0; i < container.Length; i++)
            {
                if(container[i].item == _item)
                {
                    hasItem = true;
                    container[i].AddAmount(_amount);
                    break;
                }
            }
        }

        if(!hasItem)
        {
            //Find first empty slot
            for (int i = 0; i < container.Length; i++)
            {
                if(container[i].itemId == -1)
                {
                    container[i] = new InventorySlot(_item.id, _item, _amount);
                    break;
                }
            }
        }

        //Call update inventory display event
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

    public void MoveItem(int startIndex, int endIndex)
    {
        if (container[startIndex].item == container[endIndex].item && startIndex != endIndex)
        {
            container[endIndex].AddAmount(container[startIndex].count);
            container[startIndex] = new InventorySlot();
        }
        else
        {
            InventorySlot temp = container[endIndex];
            container[endIndex] = container[startIndex];
            container[startIndex] = temp;
        }

        //Call update inventory display event
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

    public void SplitStack(int startIndex, int endIndex)
    {
        if(container[startIndex].count <= 1)
        {
            MoveItem(startIndex, endIndex);
            return;
        }

        int startSlotAmount = Mathf.CeilToInt(container[startIndex].count/2f);
        int endSlotAmount = Mathf.FloorToInt(container[startIndex].count/2f);

        container[endIndex] = new InventorySlot(container[startIndex].itemId, container[startIndex].item, container[startIndex].count);

        container[startIndex].count = startSlotAmount;
        container[endIndex].count = endSlotAmount;

        //Call update inventory display event
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

    public void RemoveItem(int index)
    {
        container[index] = new InventorySlot();
    }


    [ContextMenu("Save")]
    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();

        Debug.Log("Saved " + this + " to " + string.Concat(Application.persistentDataPath, savePath));
    }
    
    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
        else {Debug.LogWarning("No save file found!");}

        //Call update inventory display event
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

    public void OnAfterDeserialize()
    {
        for(int i = 0; i < container.Length; i++)
        {
            if (container[i].itemId == -1) 
            {
                container[i].item = null;
                container[i].count = 0;
                continue;
            }

           ItemDataObject itemFound;
            if(database.GetItem.TryGetValue(container[i].itemId, out itemFound) == false)
            {
                Debug.LogWarning("No item with the ID of " + container[i].itemId);
                continue;
            }
            container[i].item = itemFound;
        }

        //Call update inventory display event?
    }

    public void OnBeforeSerialize(){}
}

[System.Serializable]
public class InventorySlot
{
    public delegate void UpdateInventory();
    public static event UpdateInventory OnUpdateInventory;

    public int itemId = -1;
    public ItemDataObject item;
    public int count;
    public InventorySlot()
    {
        itemId = -1;
        item = null;
        count = 0;
    }
    public InventorySlot(int _id, ItemDataObject _item, int _count)
    {
        itemId = _id;
        item = _item;
        count = _count;
    }

    public void AddAmount(int value)
    {
        count += value;
    }

    public void RemoveAmount(int value)
    {
        count -= value;

        //Call update inventory display event
        if (OnUpdateInventory != null)
        {
            OnUpdateInventory();
        }
    }
}
