using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityEditor
{
    [CustomEditor(typeof(AdvancedRuleTile))]
    [CanEditMultipleObjects]
    public class AdvancedRuleTileEditor : IsometricRuleTileEditor
    {
        public Texture2D grass;
        public Texture2D dirt;
        public Texture2D empty;
        public Texture2D any;

        public override void RuleOnGUI(Rect rect, Vector3Int position, int neighbor)
        {
            switch (neighbor)
            {
                case 3:
                    GUI.DrawTexture(rect, grass);
                    return;
                case 4:
                    GUI.DrawTexture(rect, dirt);
                    return;
                case 5:
                    GUI.DrawTexture(rect, empty);
                    return;
                case 6:
                    GUI.DrawTexture(rect, any);
                    return;
            }
            base.RuleOnGUI(rect, position, neighbor);
        }
    }
}
