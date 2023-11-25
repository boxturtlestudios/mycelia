using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot : InventoryTooltipTrigger, IPointerClickHandler
{
    HotbarDisplay hotbarDisplay;
    PlayerActions playerActions;

    private void Start() 
    {
        hotbarDisplay = transform.parent.GetComponent<HotbarDisplay>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>().inventory;
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        playerActions.currentItemIndex = transform.GetSiblingIndex();
        playerActions.UpdateCurrentItem();
    }
}
