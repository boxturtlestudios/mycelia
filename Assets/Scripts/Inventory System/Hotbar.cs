using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hotbar : MonoBehaviour
{
    private int hotbarSize;
    public InventoryObject inventory;
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
        hotbarSize = transform.childCount;
        InitializeDisplay();
    }

    void InitializeDisplay()
    {
        for (int i = 0; i < hotbarSize; i++)
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

        for (int i = 0; i < hotbarSize; i++)
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
}
