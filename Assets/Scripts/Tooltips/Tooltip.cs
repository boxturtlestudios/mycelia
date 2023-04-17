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

        Vector2 position = Mouse.current.position.ReadValue();

        Canvas canvas = rectTransform.transform.parent.GetComponent<Canvas>();
        float pivotX = 0;
        float pivotY = 1;

        if(Screen.width - position.x <= rectTransform.gameObject.GetComponent<LayoutElement>().preferredWidth*canvas.scaleFactor)
        {
            pivotX = 1;
        }

        if(Screen.height + position.y <= (26*canvas.scaleFactor)+Screen.height)
        {
            pivotY = 0;
        }

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}
