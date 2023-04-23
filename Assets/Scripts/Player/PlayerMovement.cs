using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using BoxTurtleStudios.Utilities.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    public enum SlopeDirection 
    {
        southWest,
        southEast,
        northWest,
        northEast
    }


    [Header("Movement")]
    public float moveSpeed;
    [HideInInspector]
    public bool canMove = true;
    //[HideInInspector]
    public bool isOnSlope = false;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    //Angle of isometric grid lines from 90° (~Mathf.Atan(1/2))
    const float gridAngle = 26.565f;

    //Slope of 24/16
    const float southWestSlopeAngle = 56.31f;
    const float southEastSlopeAngle = 123.69f;
    const float northWestSlopeAngle = 0f;
    const float northEastSlopeAngle = 0f;
    
    private Grid tileGrid;
    private Tilemap terrain;
    private Dictionary<TileBase, SlopeDirection> slopeTiles;
    public List<Tile> southWestSlopeTiles;
    public List<Tile> southEastSlopeTiles;
    public List<Tile> northWestSlopeTiles;
    public List<Tile> northEastSlopeTiles;


    private MyceliaInputActions inputActions;
    private InputAction move;

    private Vector2 moveDirection = Vector2.zero;

    #region Initialize Input Manager

    private void OnEnable() {
        move = inputActions.Player.Move;
        move.Enable();
    }

    private void OnDisable() {
        inputActions.Disable();
    }
    #endregion

    private void Awake()
    {
        inputActions = new MyceliaInputActions();
        InitializeSlopeTiles();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Tilemap>();
        tileGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    void Update()
    {
        if(DeveloperConsoleBehaviour.Instance.devEnabled) { return; }
        HandleMovement();
    }

    void FixedUpdate()
    {
        MovePlayer(moveDirection, moveSpeed);
    }

    void HandleMovement()
    {

        if(!canMove) 
        {
            moveDirection = new Vector2(0,0);
            HandleAnimation();
            return;
        }

        /*
        // //Skewed WASD
        // Vector2 inputX = new Vector2(move.ReadValue<Vector2>().x, 0);
        // Vector2 inputY = new Vector2(0, move.ReadValue<Vector2>().y);

        // //Adjust movement vector to match isometric grid
        // Vector2 skewedInput = (Quaternion.AngleAxis(-gridAngle, Vector3.forward) * inputX) + (Quaternion.AngleAxis(-(90 - gridAngle), Vector3.forward) * inputY);

        // //Normalize the new movement vector
        // moveDirection = skewedInput.normalized;
        */

        //Skewed diagonal
        Vector2 input = new Vector2(move.ReadValue<Vector2>().x, move.ReadValue<Vector2>().y);

        // If the character is moving diagonally, rotate the movement vector by 26.57 degrees
        if (input.x != 0 && input.y != 0)
        {
            //If slope of angle is positive, subtract angle
            if (input.x / input.y > 0)
            {
                Vector2 skewedInput = Quaternion.AngleAxis(-(45 - gridAngle), Vector3.forward) * input;
                input = skewedInput;
            }
            else
            {
                Vector2 skewedInput = Quaternion.AngleAxis((45 - gridAngle), Vector3.forward) * input;
                input = skewedInput;
            }
        }

        // Normalize the movement vector and scale it by the speed
        moveDirection = input.normalized;
        HandleAnimation();
    }

    void MovePlayer(Vector2 direction, float speed = 3)
    {
        if (!canMove) 
        {
            rb.MovePosition(rb.position);
            return;
        }

        //----Adjust movement based on slope----
        TileBase currentTile = TilemapUtilities.FindCurrentTile<TileBase>(transform.position, tileGrid, terrain);

        if (currentTile != null)
        {
            SlopeDirection slopeDirection;
            if (slopeTiles.TryGetValue(currentTile, out slopeDirection))
            {
                if (slopeDirection == SlopeDirection.southWest)
                {
                    Debug.Log("On southwest slope");
                    direction = Quaternion.AngleAxis(southWestSlopeAngle - (gridAngle), Vector3.forward) * direction;
                }
                else if (slopeDirection == SlopeDirection.southEast)
                {
                    Debug.Log("On southeast slope");
                    direction = Quaternion.AngleAxis(southEastSlopeAngle - (90+(90-gridAngle)), Vector3.forward) * direction;
                }
                else if (slopeDirection == SlopeDirection.northWest)
                {

                }
                else if (slopeDirection == SlopeDirection.northEast)
                {
                    
                }
            }
        }

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void HandleAnimation()
    {
        anim.SetFloat("Speed", moveDirection.sqrMagnitude);

        if (moveDirection != Vector2.zero)
        {
            anim.SetFloat("Vertical", moveDirection.y);
        }

        //Allow tool animations to takeover
        if(!canMove) { return; }
        
        //Set sprite flip
        if(!spriteRenderer.flipX && moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void InitializeSlopeTiles()
    {
        //Initialize slope tile dictionary
        slopeTiles = new Dictionary<TileBase, SlopeDirection>();
        foreach (Tile i in southWestSlopeTiles)
        {
            slopeTiles.Add(i, SlopeDirection.southWest);
        }
        foreach (Tile i in southEastSlopeTiles)
        {
            slopeTiles.Add(i, SlopeDirection.southEast);
        }
        foreach (Tile i in northWestSlopeTiles)
        {
            slopeTiles.Add(i, SlopeDirection.northWest);
        }
        foreach (Tile i in northEastSlopeTiles)
        {
            slopeTiles.Add(i, SlopeDirection.northEast);
        }
    }
}

