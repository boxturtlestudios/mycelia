using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crop", fileName = "New Crop")]
public class CropData : ScriptableObject
{
    [ReadOnly]
    public int id;
    new public string name;
    // [TextArea(15,20)]
    // public string description;
    public ItemDataObject cropItem;
    public int cropQuantity = 1;

    public Sprite[] stages;
    [ReadOnly]
    public int growthTime;

    private void OnValidate() 
    {
        growthTime = stages.Length - 1;
    }
}
