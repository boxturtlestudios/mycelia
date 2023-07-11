using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTooltipTrigger : TooltipTriggerBase
{
    protected InventoryObject inventory;
    public static bool holdingItem = false;

    private void Start() 
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>().inventory;    
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (holdingItem)
        {
            TooltipSystem.Hide();
            return;
        }

        ItemDataObject item = inventory.container[transform.GetSiblingIndex()].item;
        if(item == null)
        {
            TooltipSystem.Hide();
            return;
        }
        
        TooltipSystem.Show(item.description, item.name);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    public static void UpdateHoldState(bool hasItemSelected)
    {
        holdingItem = hasItemSelected;
        if (holdingItem)
        {
            TooltipSystem.Hide();
            return;
        }
    }

    private IEnumerator DelayedTrigger(float delay, bool state)
    {
        yield return new WaitForSeconds(delay);
        if(state == false)
        {
            TooltipSystem.Hide();
        }
        else
        {
            Debug.LogWarning("Please specify text parameters when trying to activate a tooltip!");
        }
    }

    private IEnumerator DelayedTrigger(float delay, bool state, string content, string header = "")
    {
        yield return new WaitForSeconds(delay);
        if(state == false)
        {
            TooltipSystem.Hide();
        }
        else
        {
            TooltipSystem.Show(content, header);
        }
    }
}
