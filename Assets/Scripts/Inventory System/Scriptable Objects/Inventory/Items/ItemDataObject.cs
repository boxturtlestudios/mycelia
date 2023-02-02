using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ItemType
{
    Tool,
    Consumable,
    Generic
}
public abstract class ItemDataObject : ScriptableObject
{
    public int id;
    public Sprite icon;
    public ItemType type;
    public bool stackable = true;
    new public string name;
    [TextArea(15,20)]
    public string description;

    public virtual void Use()
    {
        Debug.Log("Used item: " + name);
    }

    public virtual void Use(Vector3 position, Grid tilegrid, Tilemap terrain)
    {
        Use();
    }
}
