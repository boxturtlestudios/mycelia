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
    private uint waterCooldown = 1*1440;
    public float waterRetentionMultiplier = 1f;
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
        canHarvest = false;
        UpdateCrop();

        return true;
    }

    private void UpdateCrop()
    {
        if(WeatherManager.Instance.currentWeatherState == WeatherState.Raining || WeatherManager.Instance.currentWeatherState == WeatherState.Storming)
        {
            lastTimeWatered = (int)TimeManager.Instance.minuteTick;
        }

        if (TimeManager.Instance.minuteTick - lastTimeWatered < waterCooldown*waterRetentionMultiplier)
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
        cropStage = Mathf.FloorToInt(Mathf.Clamp(((float)plantedProgress/(float)(currentCrop.growthTime * 1440)) * currentCrop.growthTime, 0, currentCrop.growthTime));

        if(cropStage >= currentCrop.growthTime)
        {
            canHarvest = true;
        }

        cropGraphic.sprite = currentCrop.stages[cropStage];
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
