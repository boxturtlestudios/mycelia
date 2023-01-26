using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemDataObject item;
    public int amount;
    public bool pickable = true;

    private void Update() 
    {
        transform.GetChild(0).transform.Translate(new Vector2(0, Mathf.Sin((Time.time * 5)) * 0.0005f));
    }
}
