using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
