using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemDataObject item;
    public int amount = 1;
    public bool pickable = true;
    public float moveSpeed;

    [HideInInspector]
    public bool justDropped = false;

    private void Start() 
    {
        if(item)
        {
            InitializeItem(item, amount);
        }
    }

    public void InitializeItem(ItemDataObject _item, int _amount, bool _pickable = true)
    {
        item = _item;
        amount = _amount;
        pickable = _pickable;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.icon;
    }

    private void Update() 
    {
        transform.GetChild(0).transform.Translate(new Vector2(0, Mathf.Sin((Time.time * 5)) * 0.0005f));
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(!pickable || justDropped){return;}
        if(other.CompareTag("Player"))
        {
            Vector2 difference = other.transform.position- transform.position;
            transform.position += (Vector3)(difference * (0.1f * (1f/difference.sqrMagnitude) * moveSpeed));
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            StartCoroutine("DelayDroppedToggle", 0.5f);
        }
    }

    private IEnumerator DelayDroppedToggle(float delay)
    {
        yield return new WaitForSeconds(delay);
        justDropped = false;
    }
}