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
    public GameObject farmObjectPrefab;

#region overrides
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }
#endregion

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        bool successful = base.StartUp(position, tilemap, farmObjectPrefab);

        if (GameObject.FindGameObjectWithTag("Terrain") == null) {return false;}
        Transform terrainTransform = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Transform>();
        if(terrainTransform.childCount == 0) { return false; }
        if (terrainTransform.GetChild(terrainTransform.childCount - 1) == null) {return false;}
        terrainTransform.GetChild(terrainTransform.childCount - 1).position = new Vector3(terrainTransform.GetChild(terrainTransform.childCount - 1).position.x, terrainTransform.GetChild(terrainTransform.childCount - 1).position.y, 0);
        return successful;
    }

    public GameObject FindFarmObject()
    {
        return new GameObject();
    }


    /*[MenuItem("Assets/Create/Custom Tiles/Farmland Tile")]
    public static void CreateFarmTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Farm Tile", "NewFarmTile", "asset", "Save Farm Tile", "Assets/Misc/Tile Palettes");
        if (path == "") return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FarmlandTile>(), path);
    }*/

}
