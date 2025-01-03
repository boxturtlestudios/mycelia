using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public InventoryObject inventory;
    [Header("Pickup")]
    public float pickableDistance;
    [Header("Dropping")]
    public GameObject worldItemPrefab;
    public float pickUpDelay;
    public Vector2 dropOffset;

    private void Start()
    {
        inventory.Load();
        inventory.UpdateInventory();
    }


    private void OnTriggerStay2D(Collider2D other) 
    {
        float distance = Vector2.Distance(other.gameObject.transform.position, transform.position);
        if (distance > pickableDistance) {return;}

        WorldItem item = other.gameObject.GetComponent<WorldItem>();
        if(item)
        {
            if (!item.pickable || item.justDropped) {return;}

            inventory.AddItem(item.item, item.amount);

            SoundManager.Instance.Play("Pickup");

            Destroy(other.gameObject);
        }
    }

    public void DropItem(InventorySlot slotData)
    {
        GameObject droppedItem = Instantiate(worldItemPrefab, transform.position + (Vector3)dropOffset, Quaternion.identity);
        WorldItem  droppedItemData = droppedItem.GetComponent<WorldItem>();
        droppedItemData.justDropped = true;
        droppedItemData.item = slotData.item;
        droppedItemData.amount = slotData.count;
        droppedItem.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = slotData.item.icon;
    }

    private void OnApplicationQuit() 
    {
        for (int i = 0; i < inventory.container.Length; i++)
        {
            inventory.container[i] = new InventorySlot();
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickableDistance);
    }

}
