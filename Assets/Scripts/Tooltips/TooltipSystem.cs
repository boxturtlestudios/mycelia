using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem Instance { get; private set; }
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogAssertion("Duplicate TooltipSystem destroyed!");
            Destroy(gameObject);
        }
    }

    public Tooltip tooltip;

    public static void Show(string content, string header = "")
    {
        Instance.tooltip.SetText(content, header);
        Instance.tooltip.followMouse = true;
        Instance.tooltip.gameObject.SetActive(true);
    }

    public static void Show(Vector2 pos, string content, string header = "")
    {
        Instance.tooltip.SetText(content, header);
        Instance.tooltip.followMouse = false;
        Instance.tooltip.position = pos;
        Instance.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        Instance.tooltip.gameObject.SetActive(false);
    }

}