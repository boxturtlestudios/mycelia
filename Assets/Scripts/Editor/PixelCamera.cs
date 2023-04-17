using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PixelCamera : PixelPerfectCamera
{
    [Range(0.1f, 2f)] public float scale = 1f;
    private Vector2 refResolutionRatio = new Vector2(320, 180);

    private void OnValidate() 
    {
        refResolutionX = (int)(refResolutionRatio.x * scale);
        refResolutionY = (int)(refResolutionRatio.y * scale);
    }
}
