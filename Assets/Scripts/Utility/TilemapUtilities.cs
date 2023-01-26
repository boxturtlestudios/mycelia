using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BoxTurtleStudios.Utilities.Tilemaps
{
    public class TilemapUtilities
    {
        /// <summary>
        /// Returns the tile for the given position.
        /// </summary>
        public static Tile FindCurrentTile(Vector3 _position, Grid _tileGrid, Tilemap _tilemap, int _zValue = -999)
        {
            Tile tile = null;
            Vector2 position = _position;
            Vector2 adjustedPos = position;
            Vector3Int cellLocation;
            if (_zValue != -999)
            {
                adjustedPos.y = position.y - (0.25f * _zValue);
                cellLocation = _tileGrid.WorldToCell(adjustedPos);
                cellLocation.z = _zValue;

                tile = _tilemap.GetTile<Tile>(cellLocation);
            }
            else
            {
                for (int z = 10; z > -1; z--)
                {
                    adjustedPos.y = position.y - (0.25f * z);
                    cellLocation = _tileGrid.WorldToCell(adjustedPos);
                    cellLocation.z = z;

                    tile = _tilemap.GetTile<Tile>(cellLocation);
                    if (tile != null) {break;}
                }
            }

            return tile;
        }

        /// <summary>
        /// Returns the rule tile for the given position.
        /// </summary>
        public static RuleTile FindCurrentRuleTile(Vector3 _position, Grid _tileGrid, Tilemap _tilemap, int _zValue = -999)
        {
            RuleTile tile = null;
            Vector2 playerPos = _position;
            Vector2 adjustedPos = playerPos;
            Vector3Int cellLocation;
            if (_zValue != -999)
            {
                adjustedPos.y = playerPos.y - (0.25f * _zValue);
                cellLocation = _tileGrid.WorldToCell(adjustedPos);
                cellLocation.z = _zValue;

                tile = _tilemap.GetTile<RuleTile>(cellLocation);
            }
            else
            {
                for (int z = 10; z > -1; z--)
                {
                    adjustedPos.y = playerPos.y - (0.25f * z);
                    cellLocation = _tileGrid.WorldToCell(adjustedPos);
                    cellLocation.z = z;

                    tile = _tilemap.GetTile<RuleTile>(cellLocation);
                    if (tile != null) {break;}
                }
            }

            return tile;
        }


        /// <summary>
        /// Sets the tile at the given position to the given tile.
        /// </summary>
        public static void SetCurrentTile(Tile _tileToSet, Vector3 _position, Grid _tileGrid, Tilemap _tilemap, int _zValue = -999)
        {
            Vector2 playerPos = _position;
            Vector2 adjustedPos = playerPos;
            Vector3Int cellLocation;
            if (_zValue != -999)
            {
                adjustedPos.y = playerPos.y - (0.25f * _zValue);
                cellLocation = _tileGrid.WorldToCell(adjustedPos);
                cellLocation.z = _zValue;

                _tilemap.SetTile(cellLocation, _tileToSet);
            }
            else
            {
                for (int z = 10; z > -1; z--)
                {
                    adjustedPos.y = playerPos.y - (0.25f * z);
                    cellLocation = _tileGrid.WorldToCell(adjustedPos);
                    cellLocation.z = z;

                    if(_tilemap.GetTile(cellLocation))
                    {
                        _tilemap.SetTile(cellLocation, _tileToSet);
                        break;
                    }
                }
            }
        }
    }
}

