using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot : InventoryTooltipTrigger, IPointerClickHandler
{
    HotbarDisplay hotbarDisplay;

    private void Start() 
    {
        hotbarDisplay = transform.parent.GetComponent<HotbarDisplay>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>().inventory;    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        hotbarDisplay.UpdateSelection(transform.GetSiblingIndex());
    }
}
