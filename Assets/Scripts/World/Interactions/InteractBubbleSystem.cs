using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBubbleSystem : MonoBehaviour
{
    public static InteractBubbleSystem Instance { get; private set; }
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogAssertion("Duplicate InteractBubbleSystem destroyed!");
            Destroy(gameObject);
        }
    }

    public InteractBubble bubble;
    public ItemInteractBubble itemBubble;

    public static void Show(Vector3 worldPosition, string key)
    {
        Instance.bubble.gameObject.SetActive(true);
        Instance.bubble.Setup(worldPosition, key);
        Instance.bubble.GetComponent<Animator>().Play("InteractOpen");
    }

    public static void Show(Vector3 worldPosition, string key, Sprite sprite)
    {
        Instance.itemBubble.gameObject.SetActive(true);
        Instance.itemBubble.Setup(worldPosition, key, sprite);
        Instance.itemBubble.GetComponent<Animator>().Play("InteractOpen");

    }

    public static void Hide()
    {
        if(Instance.bubble.gameObject.activeInHierarchy)
        {
            Instance.bubble.GetComponent<Animator>().Play("InteractClose");
            //Hide triggered in animation
        }
        else if(Instance.itemBubble.gameObject.activeInHierarchy)
        {
            Instance.itemBubble.GetComponent<Animator>().Play("InteractClose");
            //Hide triggered in animation
        }
    }

}
