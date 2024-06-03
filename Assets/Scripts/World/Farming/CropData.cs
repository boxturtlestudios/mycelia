using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CropType
{
    SingleHarvest,
    ContinuousGrowth,
}

[CreateAssetMenu(menuName = "Crop", fileName = "New Crop")]
public class CropData : ScriptableObject
{
    [ReadOnly]
    public int id;
    [Header("General")]
    new public string name;
    // [TextArea(15,20)]
    // public string description;
    public ItemDataObject cropItem;
    public int cropQuantity = 1;
    
    [Header("Growth Info")]
    public Sprite[] stages;
    [ReadOnly]
    public int growthTime;

    public CropType cropType;
    public int regrowTime;

    private void OnValidate() 
    {
        growthTime = stages.Length - 1;
    }
}
