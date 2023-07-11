using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode()]
public class InteractBubble : MonoBehaviour
{
    public TextMeshProUGUI textObj;
    private Vector3 currentPos;
    public Vector2 offset;

    private void Update() 
    {
        transform.position = Camera.main.WorldToScreenPoint(currentPos + (Vector3)offset);
    }

    public void Setup(Vector3 position, string key)
    {
        currentPos = position;
        textObj.text = key;
    }


    //Called by animation
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
