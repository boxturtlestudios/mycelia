using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InventoryDisplay : MonoBehaviour, IPointerClickHandler
{
    public InventoryObject inventory;

    bool hasItemSelected = false;
    bool hasSplit = false;
    GameObject dragItem = null;
    int startSlotIndex;
    int endSlotIndex;
    
    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    #region Event Subscription
    private void OnEnable() 
    {
        InventoryObject.OnUpdate += UpdateDisplay;
    }
    private void OnDisable() 
    {
        InventoryObject.OnUpdate -= UpdateDisplay;
    }
    #endregion

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>().inventory;
        InitializeDisplay();
    }

    void Update() 
    {
        if (hasItemSelected)
        {
            dragItem.transform.position = Input.mousePosition;
        }
    }

    void InitializeDisplay()
    {
        itemsDisplayed.Clear();
        for (int i = 0; i < inventory.container.Length; i++)
        {
            GameObject slot = transform.GetChild(i).gameObject;

            if(inventory.container[i].itemId == -1 || inventory.container[i] == null) 
            {
                transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
                transform.GetChild(i).GetChild(0).gameObject.SetActive(false);

                transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                Image imageRenderer = slot.transform.GetChild(0).GetComponent<Image>();
                imageRenderer.sprite = inventory.container[i].item.icon;
                imageRenderer.gameObject.SetActive(true);

                TextMeshProUGUI amountText = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                amountText.text = inventory.container[i].count.ToString();
                if (inventory.container[i].count > 1)
                {
                    amountText.gameObject.SetActive(true);
                }
                else
                {
                    amountText.gameObject.SetActive(false);
                }

            }

            itemsDisplayed.Add(slot, inventory.container[i]);
        }
    }

    public void UpdateDisplay()
    {
        //Clear dictionary
        itemsDisplayed.Clear();

        for (int i = 0; i < inventory.container.Length; i++)
        {
            if(inventory.container[i].itemId == -1 || inventory.container[i] == null) 
            {
                transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
                transform.GetChild(i).GetChild(0).gameObject.SetActive(false);

                transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                Image imageRenderer = transform.GetChild(i).GetChild(0).GetComponent<Image>();
                imageRenderer.sprite = inventory.container[i].item.icon;
                imageRenderer.gameObject.SetActive(true);

                TextMeshProUGUI amountText = transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
                amountText.text = inventory.container[i].count.ToString();
                if (inventory.container[i].count > 1)
                {
                    amountText.gameObject.SetActive(true);
                }
                else
                {
                    amountText.gameObject.SetActive(true); //Causes UI slot alignment to trigger
                    amountText.gameObject.SetActive(false);
                }
            }

            //Re-add updated slots
            itemsDisplayed.Add(transform.GetChild(i).gameObject, inventory.container[i]);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject slot = FindSlot();
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(hasSplit && hasItemSelected)
            {
                //Debug.Log("Placed Split");
                PlaceSplit(slot);
                UpdateDisplay();
            }
            else if (!hasItemSelected)
            {
                PickupItem(slot);
                //Debug.Log("Picked Item");
            }
            else
            {
                PlaceItem(slot);
                //Debug.Log("Placed Item");
                UpdateDisplay();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(!hasItemSelected)
            {
                //Debug.Log("Picked Split");
                PickupSplit(slot);
            }
            else
            {
                
            }
        }

    }

    public void PickupItem(GameObject slot)
    {
        GameObject amountDisplay = slot.transform.GetChild(1).gameObject;
        amountDisplay.SetActive(false);
        startSlotIndex = slot.transform.GetSiblingIndex();
        hasItemSelected = true;
        dragItem = slot.transform.GetChild(0).gameObject;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<GridLayoutGroup>().enabled = false;
        }
    }

    public void PlaceItem(GameObject slot)
    {   
        if (slot != null)
        {
            endSlotIndex = slot.transform.GetSiblingIndex();
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>().DropItem(inventory.container[startSlotIndex]);
            inventory.RemoveItem(startSlotIndex);
            hasItemSelected = false;
            dragItem = null;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<GridLayoutGroup>().enabled = true;
            }

            return;
        }
        hasItemSelected = false;
        dragItem = null;
        inventory.MoveItem(startSlotIndex, endSlotIndex);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<GridLayoutGroup>().enabled = true;
        }
    }

    public void PickupSplit(GameObject slot)
    {
        startSlotIndex = slot.transform.GetSiblingIndex();
        hasItemSelected = true;
        hasSplit = true;
        dragItem = slot.transform.GetChild(0).gameObject;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<GridLayoutGroup>().enabled = false;
        }
    }

    public void PlaceSplit(GameObject slot)
    {
        if (slot != null && inventory.container[slot.transform.GetSiblingIndex()].itemId == -1) //If slot is there and is empty
            {
                endSlotIndex = slot.transform.GetSiblingIndex();
            }
            else if (slot != null) //If slot is there (and has item)
            {
                hasItemSelected = false;
                hasSplit = false;
                dragItem = null;

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<GridLayoutGroup>().enabled = true;
                }

                return;
            }
            else
            {
                //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>().DropItem(inventory.container[startSlotIndex]);
                //inventory.RemoveItem(startSlotIndex);
                hasItemSelected = false;
                dragItem = null;
                return;
            }
            hasItemSelected = false;
            hasSplit = false;
            dragItem = null;
            inventory.SplitStack(startSlotIndex, endSlotIndex);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<GridLayoutGroup>().enabled = true;
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
