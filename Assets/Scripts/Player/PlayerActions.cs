using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerActions : MonoBehaviour
{
    private MyceliaInputActions inputActions;
    private InputAction use;
    private InputAction item1;
    private InputAction item2;
    private InputAction item3;
    private InputAction item4;
    private InputAction look;

    public int currentItemIndex = 0;
    public ItemDataObject currentItem = null;

    public float reachDistance;
    public Tile selectionTile;

    private Vector3Int previousSelectedPosition;
    private Vector3Int selectedCellPos;

    private Grid tileGrid;
    private Tilemap terrain;
    private Tilemap selectionTilemap;
    private InventoryObject playerInventory;
    private HotbarDisplay hotbarDisplay;

    #region Initialize Input Manager

    private void Start() {
        terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Tilemap>();
        selectionTilemap = GameObject.FindGameObjectWithTag("Selection").GetComponent<Tilemap>();
        tileGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>().inventory;
        hotbarDisplay = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<HotbarDisplay>();

        currentItem = playerInventory.container[currentItemIndex].item;
        hotbarDisplay.UpdateSelection(0);
    }

    private void Awake() {
        inputActions = new MyceliaInputActions();
    }

    private void OnEnable() {

        InventoryObject.OnUpdate += UpdateCurrentItem;

        use = inputActions.Player.Use;
        use.Enable();
        use.performed += UseItem;

        item1 = inputActions.Player.Item1;
        item2 = inputActions.Player.Item2;
        item3 = inputActions.Player.Item3;
        item4 = inputActions.Player.Item4;
        item1.Enable();
        item2.Enable();
        item3.Enable();
        item4.Enable();
        item1.performed += ToItem1;
        item2.performed += ToItem2;
        item3.performed += ToItem3;
        item4.performed += ToItem4;

        look = inputActions.Player.Look;
        look.Enable();
    }

    private void OnDisable() {
        inputActions.Disable();

        InventoryObject.OnUpdate -= UpdateCurrentItem;
    }

    #endregion


    private void Update() 
    {
        if (currentItem != null)
        {
            if(currentItem.type == ItemType.Tool)
            {
                selectionTilemap.SetTile(previousSelectedPosition, null);

                // Vector2 lookPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                // Vector2 lookVector = Vector2.ClampMagnitude(lookPos - (Vector2)transform.position, reachDistance);
                // Vector3Int playerCellPos = tileGrid.WorldToCell((Vector2)transform.position);
                // Vector3Int cellPos = tileGrid.WorldToCell(tileGrid.CellToWorld(playerCellPos) + (Vector3)lookVector);
                // previousSelectedPosition = cellPos;

                Vector2 lookPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                bool inRangeX = Mathf.Abs(tileGrid.WorldToCell(lookPos).x - tileGrid.WorldToCell(transform.position).x) <= reachDistance;
                bool inRangeY = Mathf.Abs(tileGrid.WorldToCell(lookPos).y - tileGrid.WorldToCell(transform.position).y) <= reachDistance;
                if(inRangeX && inRangeY)
                {
                    Vector3Int cellPos = tileGrid.WorldToCell(lookPos);
                    previousSelectedPosition = cellPos;

                    selectedCellPos = cellPos;
                    selectionTilemap.SetTile(cellPos, selectionTile);
                }
                else
                {
                    selectedCellPos = tileGrid.WorldToCell(transform.position);
                }
            }
        }
    }


    void UpdateCurrentItem()
    {
        currentItem = playerInventory.container[currentItemIndex].item;
        hotbarDisplay.UpdateSelection(currentItemIndex);
    }

    void UseItem(InputAction.CallbackContext context)
    {
        if(currentItem == null) {return;}

        //Use tool
        if (currentItem.type == ItemType.Tool)
        {
            currentItem.Use(tileGrid.CellToWorld(selectedCellPos), tileGrid, terrain);
        }
    }

    void ToItem1(InputAction.CallbackContext context)
    {
        //if (holdDuration > holdTime) {showTooltip();}
        currentItemIndex = 0;
        UpdateCurrentItem();
    }

    void ToItem2(InputAction.CallbackContext context)
    {
        //if (holdDuration > holdTime) {showTooltip();}
        currentItemIndex = 1;
        UpdateCurrentItem();
    }

    void ToItem3(InputAction.CallbackContext context)
    {
        //if (holdDuration > holdTime) {showTooltip();}
        currentItemIndex = 2;
        UpdateCurrentItem();
    }

    void ToItem4(InputAction.CallbackContext context)
    {
        //if (holdDuration > holdTime) {showTooltip();}
        currentItemIndex = 3;
        UpdateCurrentItem();
    }
}
