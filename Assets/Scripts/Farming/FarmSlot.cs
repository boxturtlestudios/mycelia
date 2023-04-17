using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSlot : MonoBehaviour
{
    public CropData currentCrop;
    private uint timePlanted;
    private uint plantedProgress;
    private int lastTimeWatered;
    public int cropStage { get; private set; }
    public bool isWatered { get; private set; }
    public bool canHarvest { get; private set; }
    public uint waterCooldown;
    public int sortingThreshold;

    private SpriteRenderer cropGraphic;

    public GameObject worldItemPrefab;

    #region Event Subscription
    private void OnEnable() 
    {
        TimeManager.OnTimeChanged += UpdateCrop;
    }
    private void OnDisable() 
    {
        TimeManager.OnTimeChanged -= UpdateCrop;
    }
    #endregion

    private void Start()
    {
        lastTimeWatered = (int)-waterCooldown;
        cropGraphic = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public bool PlantCrop(CropData crop)
    {
        if (currentCrop != null) {return false;}

        timePlanted = TimeManager.Instance.minuteTick;
        currentCrop = crop;
        UpdateCrop();
        return true;
    }

    public void WaterCrop()
    {
        lastTimeWatered = (int)TimeManager.Instance.minuteTick;

        UpdateCrop();
    }

    public bool HarvestCrop()
    {
        if(!canHarvest) { return false; }

        WorldItem droppedCrop = Instantiate(worldItemPrefab, transform.position, Quaternion.identity).GetComponent<WorldItem>();
        droppedCrop.item = currentCrop.cropItem;
        droppedCrop.amount = currentCrop.cropQuantity;
        droppedCrop.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = currentCrop.cropItem.icon;

        //Reseting crop
        currentCrop = null;
        timePlanted = 1;
        cropStage = 0;
        UpdateCrop();

        return true;
    }

    private void UpdateCrop()
    {
        if(WeatherManager.Instance.currentWeatherState == WeatherState.Raining || WeatherManager.Instance.currentWeatherState == WeatherState.Storming)
        {
            lastTimeWatered = (int)TimeManager.Instance.minuteTick;
        }

        if (TimeManager.Instance.minuteTick - lastTimeWatered < waterCooldown)
        {
            isWatered = true;
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else 
        {
            isWatered = false;
            transform.GetChild(1).gameObject.SetActive(false);
        }

        if (currentCrop == null) 
        {
            cropGraphic.sprite = null;
            return;
        }

        if(isWatered)
        {
            plantedProgress++;
        }
        cropStage = Mathf.FloorToInt(Mathf.Clamp(((float)plantedProgress/(float)currentCrop.growthTime)*4, 0, 4f));

        if(cropStage >= 4)
        {
            canHarvest = true;
        }

        cropGraphic.sprite = currentCrop.sprites[cropStage];
        if (cropStage >= sortingThreshold)
        {
            cropGraphic.sortingOrder = 2;
        }
        else 
        {
            cropGraphic.sortingOrder = 1;
        }
    }
}
