using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractBubble : InteractBubble
{
    public UnityEngine.UI.Image image;

    public void Setup(Vector3 position, string key, Sprite sprite)
    {
        base.Setup(position, key);
        image.sprite = sprite;
    }
}
