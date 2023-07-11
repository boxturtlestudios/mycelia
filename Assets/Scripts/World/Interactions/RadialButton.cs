using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialButton : MonoBehaviour
{
    [HideInInspector]
    public RadialOption optionInfo;

    public void Click()
    {
        optionInfo.function.Invoke(optionInfo.obj, null);
    }
}
