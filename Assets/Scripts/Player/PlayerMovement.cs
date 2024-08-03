using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using BoxTurtleStudios.Utilities.Tilemaps;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerMovement : MonoBehaviour
{
    public enum SlopeDirection 
    {
        southWest,
        southEast,
        northWest,
        northEast
    }

    public enum GroundType
    {
        grass,
        dirt,
        wood,
        snow
    }

    [Header("Movement")]
    public float moveSpeed;
    [HideInInspector]
    public bool canMove = true;
    //[HideInInspector]
    public bool onSouthEastSlope = false;
    public bool onSouthWestSlope = false;

    public PlayerState playerState = PlayerState.Walking;

    private Rigidbody2D rb;
    private PlayerAnimator animator;

    //Angle of isometric grid lines from 90° (~Mathf.Atan(1/2))
    const float gridAngle = 26.565f;

    //Slope of 24/16
    const float southWestSlopeAngle = 56.31f;
    const float southEastSlopeAngle = 123.69f;
    const float northWestSlopeAngle = 0f;
    const float northEastSlopeAngle = 0f;
    
    private Grid tileGrid;
    private Tilemap terrain;

    public List<TileBase> grassTiles;
    public List<TileBase> dirtTiles;
    public List<TileBase> woodTiles;
    private TileBase currentTile;
    private GroundType currentGroundType;
    private GroundType? groundTypeOverride;

    private MyceliaInputActions inputActions;
    private InputAction move;

    private Vector2 moveDirection = Vector2.zero;

    public float stepFrequency = 0.4f;
    private float elapsedTime = 0f;

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
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<PlayerAnimator>();
        
        terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Tilemap>();
        tileGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    void Update()
    {
        if(DeveloperConsoleBehaviour.Instance.devEnabled) { return; }
        HandleMovement();

        canMove = playerState != PlayerState.UsingTool && playerState != PlayerState.Busy;

        GroundType groundType = GroundTile(currentTile);

        if(groundTypeOverride != null) { groundType = (GroundType)groundTypeOverride; }

        if(moveDirection.sqrMagnitude > 0.01f)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= stepFrequency)
            {
                switch (groundType)
                {
                    case GroundType.grass:
                        SoundManager.Instance.Play("Grass");
                    break;

                    case GroundType.dirt:
                        SoundManager.Instance.Play("Dirt");
                    break;

                    case GroundType.wood:
                        SoundManager.Instance.Play("Wood");
                    break;
                }
                elapsedTime = 0f;
            }
        }
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
            animator.SetDirection(moveDirection, DirectionType.Movement);
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
        animator.SetDirection(moveDirection, DirectionType.Movement);
    }

    void MovePlayer(Vector2 direction, float speed = 3)
    {
        if (!canMove) 
        {
            rb.MovePosition(rb.position);
            return;
        }

        if (direction.x != 0 && direction.y != 0)
        {
            if (onSouthEastSlope)
            {
                direction = Quaternion.AngleAxis(southEastSlopeAngle - (90+(90-gridAngle)), Vector3.forward) * direction;
            }
            else if (onSouthWestSlope)
            {
                direction = Quaternion.AngleAxis(southWestSlopeAngle - (gridAngle), Vector3.forward) * direction;
            }
        }

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private GroundType GroundTile(TileBase current)
    {
        foreach(TileBase tile in grassTiles)
        {
            if(tile == current) { return GroundType.grass; }
        }
        foreach(TileBase tile in dirtTiles)
        {
            if(tile == current) { return GroundType.dirt; }
        }
        foreach(TileBase tile in woodTiles)
        {
            if(tile == current) { return GroundType.wood; }
        }
        return GroundType.grass;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("SouthEastSlope"))
        {
            onSouthWestSlope = false;
            onSouthEastSlope = true;
        }
        else if (other.CompareTag("SouthWestSlope"))
        {
            onSouthEastSlope = false;
            onSouthWestSlope = true;
        }

        if(other.CompareTag("Stone Floor"))
        {
            groundTypeOverride = GroundType.wood; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("SouthEastSlope") || other.CompareTag("SouthWestSlope"))
        {
            onSouthWestSlope = false;
            onSouthEastSlope = false;
        }

        if(other.CompareTag("Stone Floor"))
        {
            groundTypeOverride = null; 
        }
    }

}

