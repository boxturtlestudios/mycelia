using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIControl : MonoBehaviour
{
    public static UIControl Instance { get; private set; }
    private void Awake() 
    {
        inputActions = new MyceliaInputActions();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogAssertion("Duplicate UIControl destroyed!");
            Destroy(gameObject);
        }
    }

    private MyceliaInputActions inputActions;
    private InputAction toggleInventory;
    private InputAction toggleBook;

    private PlayerMovement playerMovement;

    [Header("UI Elements")]
    public GameObject background;
    public GameObject inventory;
    public GameObject hotbar;
    public GameObject book;
    public GameObject houseCustomization;

    [Header("Logic")]
    public static bool inventoryEnabled = false;
    private bool bookEnabled = false;
    public bool customizationEnabled = false;

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
    #endregion

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void ToggleInventory(InputAction.CallbackContext context)
    {
        if(DeveloperConsoleBehaviour.Instance.devEnabled) { return; }
        if(customizationEnabled) { return; }
        if(playerMovement.playerState == PlayerState.Interacting) { return; }
        inventoryEnabled = !inventoryEnabled;
        if(inventoryEnabled) { SoundManager.Instance.Play("Inventory Open"); }
        else { SoundManager.Instance.Play("Inventory Close"); }
        UpdateUI();
    }

    void ToggleBook(InputAction.CallbackContext context)
    {
        if(DeveloperConsoleBehaviour.Instance.devEnabled) { return; }

        if (!inventoryEnabled) {return;}

        bookEnabled = !bookEnabled;
        UpdateUI();
    }

    public void OpenHouseCustomization()
    {
        customizationEnabled = true;
        UpdateUI();
        playerMovement.playerState = PlayerState.Busy;
        houseCustomization.GetComponent<HouseCustomizationMenu>().StartCustomization();
        CameraManager.Instance.ViewHouse();
    }

    public void CloseHouseCustomization()
    {
        customizationEnabled = false;
        UpdateUI();
        playerMovement.playerState = PlayerState.Walking;
        CameraManager.Instance.ViewPlayer();
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

        houseCustomization.SetActive(customizationEnabled);

        hotbar.SetActive(!(customizationEnabled || inventoryEnabled));
        hotbar.GetComponent<HotbarDisplay>().UpdateDisplay();


        if(inventoryEnabled || customizationEnabled)
        {
            playerMovement.playerState = PlayerState.Busy;
        }
        else
        {
            playerMovement.playerState = PlayerState.Walking;
            TooltipSystem.Hide();
        }
    }
}
