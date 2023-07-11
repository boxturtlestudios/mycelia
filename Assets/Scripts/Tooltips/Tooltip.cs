using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public bool followMouse = true;
    public Vector2 position;
    private Vector2 pivot;

    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header = "")
    {
        if(string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    private void Update()
    {
        if(Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
        }

        if (!followMouse) 
        { 
            pivot = new Vector2(0.25f, -0.2f);
        }
        else
        {
            position = Mouse.current.position.ReadValue();

            Canvas canvas = rectTransform.transform.parent.GetComponent<Canvas>();
            pivot.x = 0;
            pivot.y = 1;

            if(Screen.width - position.x <= rectTransform.gameObject.GetComponent<LayoutElement>().preferredWidth*canvas.scaleFactor)
            {
                pivot.x = 1;
            }

            if(Screen.height + position.y <= (26*canvas.scaleFactor)+Screen.height)
            {
                pivot.y = 0;
            }
        }

        rectTransform.pivot = pivot;
        transform.position = position;
    }
}
