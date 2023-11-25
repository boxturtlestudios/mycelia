using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuSystem : MonoBehaviour
{
    public static RadialMenuSystem Instance { get; private set; }
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogAssertion("Duplicate RadialMenuSystem destroyed!");
            Destroy(gameObject);
        }
    }

    public RadialWheel radialWheel;

    public static void Show(Vector3 worldPosition, RadialMenu menu)
    {
        Instance.radialWheel.Open(worldPosition, menu);
        SoundManager.Instance.Play("Open Radial");
    }

    public static void Hide()
    {
        Instance.radialWheel.Close();
        SoundManager.Instance.Play("Close Radial");
    }

}
