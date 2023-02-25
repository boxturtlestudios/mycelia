using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(menuName = "Custom Tiles/Advanced Rule Tile", fileName = "New Advanced Rule Tile")]
public class AdvancedRuleTile : IsometricRuleTile<AdvancedRuleTile.Neighbor> {
    public bool alwaysConnect;
    public TileBase[] dirtTiles;
    public TileBase[] grassTiles;
    public bool checkSelf;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Grass = 1;
        new public const int NotThis = 2;
        public const int Any = 3;
        public const int Dirt = 4;
        public const int Nothing = 5;
        new public const int This = 6;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This: return Check_This(tile);
            case Neighbor.NotThis: return Check_NotThis(tile);
            case Neighbor.Any: return Check_Any(tile);
            case Neighbor.Dirt: return Check_Dirt(tile);
            case Neighbor.Nothing: return Check_Nothing(tile);
            case Neighbor.Grass: return Check_Grass(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    bool Check_This(TileBase tile)
    {
        if(!alwaysConnect) return tile == this;
        else return dirtTiles.Contains(this) || tile == this;
    }

    bool Check_NotThis(TileBase tile)
    {
        return tile != this;
    }

    bool Check_Any(TileBase tile)
    {
        if(checkSelf) return tile != null;
        else return tile != null || tile != this;
    }

    bool Check_Dirt(TileBase tile)
    {
        return dirtTiles.Contains(tile);
    }

    bool Check_Nothing(TileBase tile)
    {
        return tile == null;
    }

    bool Check_Grass(TileBase tile)
    {
        return tile == this || grassTiles.Contains(tile);
    }

}