using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public Transform parentAfterDrag;
    PointerEventData pointerEventData = new PointerEventData(null);

    bool isSelected;

    /*public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");

        if (FindSlot() != null)
        {
            parentAfterDrag = FindSlot().transform;
        }
        transform.SetParent(parentAfterDrag);
    }*/

    private void Update()
    {
        if(isSelected)
        {
            Debug.Log("Dragging");
            transform.position = Input.mousePosition;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int startSlotIndex = 0;
        int endSlotIndex;

        if (FindSlot() == transform.parent.gameObject)
        {
            Debug.Log("Begin drag");

            startSlotIndex = FindSlot().transform.GetSiblingIndex();

            GameObject amountDisplay = transform.parent.GetChild(1).gameObject;
            amountDisplay.SetActive(false);

            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            isSelected = true;
        }
        else
        {
            Debug.Log("End drag");


            if (FindSlot() != null)
            {
                endSlotIndex = FindSlot().transform.GetSiblingIndex();
                parentAfterDrag = FindSlot().transform;
            }
            else
            {
                endSlotIndex = startSlotIndex;
            }

            transform.SetParent(parentAfterDrag);
            transform.SetAsFirstSibling();
            isSelected = false;

            transform.parent.parent.GetComponent<InventoryDisplay>().inventory.MoveItem(startSlotIndex, endSlotIndex);

            //transform.parent.GetComponent<InventoryDisplay>().UpdateDisplay();
        }
    }

    public GameObject FindSlot()
    {
         
        PointerEventData pointerData = new PointerEventData (EventSystem.current)
        {
            pointerId = -1,
        };
         
        pointerData.position = Input.mousePosition;
 
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
         
        foreach (RaycastResult i in results)
        {
            //Debug.Log(i.gameObject);
            if (i.gameObject.CompareTag("Slot"))
            {
                //Debug.Log("Found slot: " + i.gameObject);
                return i.gameObject;
            }
        }

        //Debug.LogWarning("No slot found!");
        return null;
    }
}
