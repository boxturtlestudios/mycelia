using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public InventoryObject inventory;
    [Header("Dropping")]
    public GameObject worldItemPrefab;
    public float pickUpDelay;
    public Vector2 dropOffset;
    private AudioSource audioSource;
    public AudioClip pickupSound;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        WorldItem item = other.GetComponent<WorldItem>();
        if(item)
        {
            if (!item.pickable) {return;}

            inventory.AddItem(item.item, item.amount);

            audioSource.clip = pickupSound;
            audioSource.Play();

            Destroy(other.gameObject);
        }
    }

    public void DropItem(InventorySlot _slotData)
    {
        StartCoroutine(DropItemCo(_slotData));
    }

    IEnumerator DropItemCo(InventorySlot slotData)
    {
        GameObject droppedItem = Instantiate(worldItemPrefab, transform.position + (Vector3)dropOffset, Quaternion.identity);
        WorldItem  droppedItemData = droppedItem.GetComponent<WorldItem>();
        droppedItemData.pickable = false;
        droppedItemData.item = slotData.item;
        droppedItemData.amount = slotData.count;
        droppedItem.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = slotData.item.icon;

        yield return new WaitForSeconds(pickUpDelay);

        droppedItem.GetComponent<WorldItem>().pickable = true;
    }

    private void OnApplicationQuit() 
    {
        for (int i = 0; i < inventory.container.Length; i++)
        {
            inventory.container[i] = new InventorySlot();
        }
    }
}
