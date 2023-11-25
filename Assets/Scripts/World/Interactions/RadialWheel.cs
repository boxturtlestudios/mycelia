using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadialWheel : MonoBehaviour
{
    public float radius;
    private Vector3 currentPos;

    public Vector2 offset;
    public GameObject optionPrefab;

    //public RadialMenu currentMenu;

    private void Update() 
    {
        transform.position = Camera.main.WorldToScreenPoint(currentPos + (Vector3)offset);
        SpaceItems();
    }

    public void Open(Vector3 worldPosition, RadialMenu menu)
    {   
        currentPos = worldPosition;

        foreach(RadialOption i in menu.options)
        {
            GameObject optionObject = Instantiate(optionPrefab, Vector3.zero, Quaternion.identity, transform);
            optionObject.GetComponentInChildren<TextMeshProUGUI>().text = i.label;
            optionObject.GetComponent<RadialButton>().optionInfo = i;
        }

        gameObject.SetActive(true);
        GetComponent<Animator>().Play("RadialOpen");
    }

    public void Close()
    {
        if(this.gameObject.activeInHierarchy)
        {
            GetComponent<Animator>().Play("RadialClose");
        }
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void SpaceItems()
    {
        int childCount = transform.childCount;
        float angleDelta = 360f / childCount;
        float currentAngle = 0f;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector2 position = GetPositionOnCircle(currentAngle);
            child.GetComponent<RectTransform>().anchoredPosition = position;
            currentAngle += angleDelta;
        }
    }

    private Vector2 GetPositionOnCircle(float angle)
    {
        float radian = Mathf.Deg2Rad * angle;
        float x = GetComponent<RectTransform>().anchoredPosition.x + radius * Mathf.Cos(radian);
        float y = GetComponent<RectTransform>().anchoredPosition.y + radius * Mathf.Sin(radian);
        return new Vector2(x, y);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class RadialOption {
    public string label;
    public object obj;
    public System.Reflection.MethodInfo function;
    public string functionName;
    /* public delegate void Function();
    public Function function; */
}

[System.Serializable]
public class RadialMenu {
    public RadialOption[] options;
}
