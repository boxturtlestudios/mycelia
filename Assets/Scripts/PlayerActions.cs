using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using BoxTurtleStudios.Utilities.Tilemaps;

public class PlayerActions : MonoBehaviour
{
    private MyceliaInputActions inputActions;
    private InputAction use;

    private Grid tileGrid;
    private Tilemap terrain;
    public Tile dirtTile;
    public RuleTile grassTile;

    #region Initialize Input Manager

    private void Start() {
        terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Tilemap>();
        tileGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    private void Awake() {
        inputActions = new MyceliaInputActions();
    }

    private void OnEnable() {
        use = inputActions.Player.Use;
        use.Enable();
        use.performed += UseItem;
    }

    private void OnDisable() {
        inputActions.Disable();
    }

    #endregion

    void UseItem(InputAction.CallbackContext context)
    {
        //Dig
        UseShovel();
        
    }

    void UseShovel()
    {
        if(TilemapUtilities.FindCurrentRuleTile(transform.position, tileGrid, terrain) == grassTile)
        {
            TilemapUtilities.SetCurrentTile(dirtTile, transform.position, tileGrid, terrain);
        }

        Debug.Log("Dug");
    }

#region OldFunctions
    /*Tile FindCurrentTile(int zValue = -999)
    {
        Tile tile = null;
        Vector2 playerPos = transform.position;
        Vector2 adjustedPos = playerPos;
        Vector3Int cellLocation;
        if (zValue != -999)
        {
            adjustedPos.y = playerPos.y - (0.25f * zValue);
            cellLocation = tileGrid.WorldToCell(adjustedPos);
            cellLocation.z = zValue;

            tile = terrain.GetTile<Tile>(cellLocation);
        }
        else
        {
            for (int z = 10; z > -1; z--)
            {
                adjustedPos.y = playerPos.y - (0.25f * z);
                cellLocation = tileGrid.WorldToCell(adjustedPos);
                cellLocation.z = z;

                tile = terrain.GetTile<Tile>(cellLocation);
                if (tile != null) {break;}
            }
        }

        return tile;
    }

    RuleTile FindCurrentRuleTile(int zValue = -999)
    {
        RuleTile tile = null;
        Vector2 playerPos = transform.position;
        Vector2 adjustedPos = playerPos;
        Vector3Int cellLocation;
        if (zValue != -999)
        {
            adjustedPos.y = playerPos.y - (0.25f * zValue);
            cellLocation = tileGrid.WorldToCell(adjustedPos);
            cellLocation.z = zValue;

            tile = terrain.GetTile<RuleTile>(cellLocation);
        }
        else
        {
            for (int z = 10; z > -1; z--)
            {
                adjustedPos.y = playerPos.y - (0.25f * z);
                cellLocation = tileGrid.WorldToCell(adjustedPos);
                cellLocation.z = z;

                tile = terrain.GetTile<RuleTile>(cellLocation);
                if (tile != null) {break;}
            }
        }

        return tile;
    }

    void SetCurrentTile(Tile tileToSet, int zValue = -999)
    {
        Vector2 playerPos = transform.position;
        Vector2 adjustedPos = playerPos;
        Vector3Int cellLocation;
        if (zValue != -999)
        {
            adjustedPos.y = playerPos.y - (0.25f * zValue);
            cellLocation = tileGrid.WorldToCell(adjustedPos);
            cellLocation.z = zValue;

            terrain.SetTile(cellLocation, tileToSet);
        }
        else
        {
            for (int z = 10; z > -1; z--)
            {
                adjustedPos.y = playerPos.y - (0.25f * z);
                cellLocation = tileGrid.WorldToCell(adjustedPos);
                cellLocation.z = z;

                if(terrain.GetTile(cellLocation))
                {
                    terrain.SetTile(cellLocation, tileToSet);
                    break;
                }
            }
        }
    }*/
#endregion
}
