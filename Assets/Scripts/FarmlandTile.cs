using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Custom Tiles/Farmland Tile", fileName = "New Farmland Tile")]
public class FarmlandTile : Tile
{
#region overrides
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        Debug.Log("RefreshTile() farmtile");
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        Debug.Log("GetTileData() farmtile");
        base.GetTileData(position, tilemap, ref tileData);
    }
#endregion

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        Debug.Log("StartUp() farmtile");
        return base.StartUp(position, tilemap, go);
    }

    public void TestTile()
    {
        Debug.Log("Hi im a farm tile!");
    }

    /*[MenuItem("Assets/Create/Custom Tiles/Farmland Tile")]
    public static void CreateFarmTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Farm Tile", "NewFarmTile", "asset", "Save Farm Tile", "Assets/Misc/Tile Palettes");
        if (path == "") return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FarmlandTile>(), path);
    }*/

}
