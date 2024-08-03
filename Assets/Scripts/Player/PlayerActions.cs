using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using BoxTurtleStudios.Utilities;
using System.Linq;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerActions : MonoBehaviour
{
    private MyceliaInputActions inputActions;
    private InputAction use;
    private InputAction item1;
    private InputAction item2;
    private InputAction item3;
    private InputAction item4;
    private InputAction cycleItem;
    private InputAction look;
    private InputAction showSelectionButton;
    private InputAction interact;

    public int currentItemIndex = 0;
    public ItemDataObject currentItem = null;
    private bool canUseItem = true;

    public float reachDistance;
    public Tile selectionTile;
    public bool showSelection;
    private int hotbarLength;

    private Vector3Int previousSelectedPosition;
    private Vector3Int selectedCellPos;
    private Vector2 facingDir;

    private Grid tileGrid;
    private Tilemap terrain;
    private Tilemap selectionTilemap;
    private InventoryObject playerInventory;
    private HotbarDisplay hotbarDisplay;

    private PlayerAnimator animator;
    private PlayerMovement playerMovement;

    private Interactable currentInteractable;
    private List<Interactable> interactables = new List<Interactable>();
    private Interactable previousInteractable;


    #region Initialize Input Manager


    private void Awake() {
        inputActions = new MyceliaInputActions();
    }

    private void OnEnable() {

        InventoryObject.OnUpdate += UpdateCurrentItem;

        use = inputActions.Player.Use;
        use.Enable();
        use.performed += (InputAction.CallbackContext context) => StartCoroutine(UseItem());

        item1 = inputActions.Player.Item1;
        item2 = inputActions.Player.Item2;
        item3 = inputActions.Player.Item3;
        item4 = inputActions.Player.Item4;
        cycleItem = inputActions.Player.CycleItem;
        item1.Enable();
        item2.Enable();
        item3.Enable();
        item4.Enable();
        cycleItem.Enable();
        item1.performed += (InputAction.CallbackContext context) => ChangeItem(0);
        item2.performed += (InputAction.CallbackContext context) => ChangeItem(1);
        item3.performed += (InputAction.CallbackContext context) => ChangeItem(2);
        item4.performed += (InputAction.CallbackContext context) => ChangeItem(3);
        cycleItem.performed += CycleItem;

        interact = inputActions.Player.Interact;
        interact.Enable();
        interact.performed += (InputAction.CallbackContext context) => { InteractWithObject(); };



        showSelectionButton = inputActions.Player.ShowSelection;
        showSelectionButton.Enable();
        showSelectionButton.started += (InputAction.CallbackContext context) => showSelection = true;
        showSelectionButton.canceled += (InputAction.CallbackContext context) => showSelection = false;

        look = inputActions.Player.Look;
        look.Enable();
    }

    private void OnDisable() {
        inputActions.Disable();

        InventoryObject.OnUpdate -= UpdateCurrentItem;
    }

    #endregion

    private void Start() {
        terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Tilemap>();
        selectionTilemap = GameObject.FindGameObjectWithTag("Selection").GetComponent<Tilemap>();
        tileGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>().inventory;
        hotbarDisplay = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<HotbarDisplay>();

        currentItem = playerInventory.container[currentItemIndex].item;
        hotbarDisplay.UpdateSelection(0);

        hotbarLength = hotbarDisplay.gameObject.transform.childCount;

        animator = GetComponent<PlayerAnimator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() 
    {
        canUseItem = (playerMovement.playerState != PlayerState.UsingTool) && (playerMovement.playerState != PlayerState.Interacting) && (playerMovement.playerState != PlayerState.Busy);

        if(DeveloperConsoleBehaviour.Instance.devEnabled) { return; }
        if (UIControl.inventoryEnabled) { return; }

        selectionTilemap.SetTile(previousSelectedPosition, null);

        Vector2 lookPos = Camera.main.ScreenToWorldPoint(look.ReadValue<Vector2>());

        bool inRangeX = Mathf.Abs(tileGrid.WorldToCell(lookPos).x - tileGrid.WorldToCell(transform.position).x) <= reachDistance;
        bool inRangeY = Mathf.Abs(tileGrid.WorldToCell(lookPos).y - tileGrid.WorldToCell(transform.position).y) <= reachDistance;
        if(inRangeX && inRangeY)
        {
            Vector3Int cellPos = tileGrid.WorldToCell(lookPos);
            previousSelectedPosition = cellPos;

            selectedCellPos = cellPos;                                                 //Offset to center look position to tile
            facingDir = (tileGrid.CellToWorld(selectedCellPos) - transform.position) + new Vector3(0, tileGrid.cellSize.y/2, 0);
        }
        else
        {
            selectedCellPos = tileGrid.WorldToCell(transform.position);
        }

        if(!showSelection) { return; }
        if (inRangeX && inRangeY)
        {
            selectionTilemap.SetTile(selectedCellPos, selectionTile);
        }

        else
        {
        }
    }


    public void UpdateCurrentItem()
    {
        if(UIControl.Instance.customizationEnabled) { return; }
        currentItem = playerInventory.container[currentItemIndex].item;
        hotbarDisplay.UpdateSelection(currentItemIndex);
    }

    IEnumerator UseItem()
    {
        if(DeveloperConsoleBehaviour.Instance.devEnabled) { yield break; }
        if (UIControl.inventoryEnabled) { yield break; }
        if (!canUseItem) { yield break; }
        
        if(currentItem == null || (currentItem.type != ItemType.Tool && currentItem.type != ItemType.Seeds)) 
        {
            //Try to harvest crop
            HarvestCrop(tileGrid.CellToWorld(selectedCellPos), tileGrid, terrain);
            yield break;
        }

        //Use tool
        if (currentItem.type == ItemType.Tool)
        {
            playerMovement.playerState = PlayerState.UsingTool;
            animator.playerState = PlayerState.UsingTool;

            yield return new WaitForSeconds(0.15f);

            string trigger;
            bool successful = currentItem.Use(tileGrid.CellToWorld(selectedCellPos), tileGrid, terrain, out trigger);
            animator.SetDirection(facingDir.normalized, DirectionType.Facing);
            animator.Trigger(trigger);
            if (successful)
            {
                SoundManager.Instance.Play(trigger);
            }

            yield return new WaitForSeconds(currentItem.useCooldown);

            playerMovement.playerState = PlayerState.Walking;
            animator.playerState = PlayerState.Walking;
        }
        //Use Seeds
        else if (currentItem.type == ItemType.Seeds)
        {
            if(currentItem.Use(tileGrid.CellToWorld(selectedCellPos), tileGrid, terrain))
            {
                animator.playerState = PlayerState.UsingTool;
                playerMovement.playerState = PlayerState.UsingTool;

                playerInventory.container[currentItemIndex].RemoveAmount(1);

                yield return new WaitForSeconds(currentItem.useCooldown);
                animator.playerState = PlayerState.Walking;
                playerMovement.playerState = PlayerState.Walking;
            }
        }
    }

    private void HarvestCrop(Vector3 position, Grid tileGrid, Tilemap terrain)
    {
        Transform tilemapTransform = terrain.gameObject.transform;
            for (int i = 0; i < tilemapTransform.childCount; i++)
            {
                if(tilemapTransform.GetChild(i).position == position)
                {
                    FarmSlot farmSlot = tilemapTransform.GetChild(i).GetComponent<FarmSlot>();
                    farmSlot.HarvestCrop();
                    break;
                }
            }
    }

    public void ChangeItem(int index)
    {
        if(playerMovement.playerState == PlayerState.Interacting || playerMovement.playerState == PlayerState.Busy) { return ;}
        //if (holdDuration > holdTime) {showTooltip();}
        currentItemIndex = index;
        UpdateCurrentItem();
    }

    public void CycleItem(InputAction.CallbackContext context)
    {
        if(playerMovement.playerState == PlayerState.Interacting || playerMovement.playerState == PlayerState.Busy) { return ;}

        float scrollDelta = context.ReadValue<Vector2>().y;
        
        if (scrollDelta > 0)
        {
            currentItemIndex = MathB.Mod(currentItemIndex - 1, hotbarLength);
        }
        else if (scrollDelta < 0)
        {
            currentItemIndex = MathB.Mod(currentItemIndex + 1, hotbarLength);
        }

        UpdateCurrentItem();
    }

    // Called by interact input key
    private void InteractWithObject()
    {
        if(playerMovement.playerState == PlayerState.Interacting || playerMovement.playerState == PlayerState.Busy || playerMovement.playerState == PlayerState.UsingTool) { return ;}
        if(interactables.Count <= 0) { return; }
        currentInteractable.Interact();
    }

    // private void OnTriggerEnter2D(Collider2D other) 
    // {
    //     if(other.GetComponent<Interactable>())
    //     {
        
    //         Interactable otherInteractable = other.GetComponent<Interactable>();

    //         if(otherInteractable is ItemInteractable)
    //         {
    //             //If the player is not holding an acceptable item
    //             if(!((ItemInteractable)otherInteractable).acceptableItems.Contains(currentItem))
    //             {
    //                 Debug.Log("Interactable requires an item");
    //                 return;
    //             }
    //         }

    //         interactables.Add(otherInteractable);
    //         interactables = interactables.OrderBy( x => Vector2.Distance(this.transform.position, x.transform.position) ).ToList();
            
    //         if(interactables.Count > 0) { currentInteractable = interactables[0]; }
    //         else { currentInteractable = null; return; }

    //         //Determine if the interactable is an item interactable
    //         if(currentInteractable is ItemInteractable)
    //         {
    //             Debug.Log("Displaying neccesary item");
    //             InteractBubbleSystem.Show(currentInteractable.transform.position + (Vector3)currentInteractable.bubbleOffset, "X");
    //         }
    //         else
    //         {
    //             InteractBubbleSystem.Show(currentInteractable.transform.position + (Vector3)currentInteractable.bubbleOffset, "X");
    //         }
    //     }
    // }

    private void OnTriggerStay2D(Collider2D other)
    {
        Interactable otherInteractable = other.GetComponent<Interactable>();
        if (!otherInteractable) { return; }

        bool validInteraction = otherInteractable is ItemInteractable ? ((ItemInteractable)otherInteractable).acceptableItems.Contains(currentItem) : true;

        //Existing interactable
        if (interactables.Contains(otherInteractable))
        {
            if(!validInteraction)
            {
                interactables.Remove(otherInteractable);
                if(previousInteractable == otherInteractable) { InteractBubbleSystem.Hide(); }
            }
        }
        //New interactable
        else
        {
            if(validInteraction)
            {
                interactables.Add(otherInteractable);                
            }
        }

        interactables = interactables.OrderBy(x => Vector2.Distance(this.transform.position, x.transform.position)).ToList();
        currentInteractable = interactables.Count > 0 ? interactables[0] : null;

        if (currentInteractable != previousInteractable && validInteraction)
        {
            if(currentInteractable is ItemInteractable)
            {
                InteractBubbleSystem.Show(currentInteractable.transform.position + (Vector3)currentInteractable.bubbleOffset, "X", currentItem.icon);
            }
            else
            {
                InteractBubbleSystem.Show(currentInteractable.transform.position + (Vector3)currentInteractable.bubbleOffset, "X");
            }
        }

        previousInteractable = currentInteractable;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        Interactable otherInteractable = other.GetComponent<Interactable>();
        if (!otherInteractable) { return; }

        if(!interactables.Contains(otherInteractable)) { return; }

        if(otherInteractable == currentInteractable)
        {
            currentInteractable.Disengage();
            previousInteractable = null;
        }

        interactables.Remove(otherInteractable);
        interactables = interactables.OrderBy( x => Vector2.Distance(this.transform.position, x.transform.position) ).ToList();
        
        if(interactables.Count > 0)
        {
            Debug.Log("Switching selected objects");
            currentInteractable = interactables[0]; 
            InteractBubbleSystem.Show(currentInteractable.transform.position + (Vector3)currentInteractable.bubbleOffset, "X");
        }
        else 
        { 
            currentInteractable = null;
            InteractBubbleSystem.Hide();
            Debug.Log("Hiding bubble bc no interactables");
        }
    }



    private void OnDrawGizmos() 
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3)facingDir);
        //Gizmos.DrawWireSphere(transform.position + new Vector3(0.4f, 0, 0), 0.5f);
    }
}
