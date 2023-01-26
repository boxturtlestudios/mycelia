using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{


    [Header("UI Elements")]
    public GameObject background;
    public GameObject inventory;
    public GameObject hotbar;
    public GameObject book;

    [Header("Logic")]
    private bool bookEnabled = false;

    [Header("Resources")]
    public Sprite inventoryWithBook;
    public Sprite inventoryWithoutBook;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            background.SetActive(!background.activeInHierarchy);

            inventory.GetComponent<Image>().sprite = bookEnabled? inventoryWithBook : inventoryWithoutBook;
            inventory.SetActive(!inventory.activeInHierarchy);
            inventory.GetComponentInChildren<InventoryDisplay>().UpdateDisplay();
        }
    }
}
