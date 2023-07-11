using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ItemType
{
    Tool,
    Consumable,
    Generic,
    Seeds
}
public abstract class ItemDataObject : ScriptableObject
{
    [ReadOnly]
    public int id;
    public Sprite icon;
    public ItemType type;
    public bool stackable = true;
    new public string name;
    [TextArea(15,20)]
    public string description;
    public float useCooldown;

    //protected Animator playerAnim;

    protected GameObject playerObj;

    public virtual bool Use()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");   
        return true; 
        //Debug.Log("Used item: " + name);
    }

    public virtual bool Use(Vector3 position, Grid tilegrid, Tilemap terrain)
    {
        Use();
        return true; 
    }

    public virtual bool Use(Vector3 position, Grid tilegrid, Tilemap terrain, out string trigger)
    {
        Use();
        trigger = null;
        return true; 
    }
}
