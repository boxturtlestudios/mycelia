using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIControl : MonoBehaviour
{
    private MyceliaInputActions inputActions;
    private InputAction toggleInventory;
    private InputAction toggleBook;

    private PlayerMovement playerMovement;

    [Header("UI Elements")]
    public GameObject background;
    public GameObject inventory;
    public GameObject hotbar;
    public GameObject book;

    [Header("Logic")]
    public static bool inventoryEnabled = false;
    private bool bookEnabled = false;

    [Header("Resources")]
    public Sprite inventoryWithBook;
    public Sprite inventoryWithoutBook;

    #region Initialize Input Manager
    private void OnEnable() {
        toggleInventory = inputActions.Player.ToggleInventory;
        toggleInventory.Enable();
        toggleBook = inputActions.Player.ToggleBook;
        toggleBook.Enable();

        toggleInventory.performed += ToggleInventory;
        toggleBook.performed += ToggleBook;
    }

    private void OnDisable() {
        inputActions.Disable();
    }

    private void Awake()
    {
        inputActions = new MyceliaInputActions();
    }
    #endregion

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void ToggleInventory(InputAction.CallbackContext context)
    {
        inventoryEnabled = !inventoryEnabled;
        UpdateUI();
    }

    void ToggleBook(InputAction.CallbackContext context)
    {
        if (!inventoryEnabled) {return;}

        bookEnabled = !bookEnabled;
        UpdateUI();
    }

    void UpdateUI()
    {
        // //Call update inventory display event
        // if (OnUpdate != null)
        // {
        //     OnUpdate();
        // }

        background.SetActive(inventoryEnabled);

        inventory.GetComponent<Image>().sprite = bookEnabled? inventoryWithoutBook : inventoryWithBook;
        inventory.SetActive(inventoryEnabled);
        inventory.GetComponentInChildren<InventoryDisplay>().UpdateDisplay();

        book.SetActive(bookEnabled && inventoryEnabled);

        hotbar.SetActive(!inventoryEnabled);
        hotbar.GetComponent<HotbarDisplay>().UpdateDisplay();


        if(inventoryEnabled)
        {
            playerMovement.canMove = false;
        }
        else
        {
            playerMovement .canMove = true;
            TooltipSystem.Hide();
        }
    }
}
