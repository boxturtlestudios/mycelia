using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHittable : MonoBehaviour
{
    public int objectHealth;
    protected int currentHealth;
    public ItemDataObject dropItem;
    public GameObject worldItemPrefab;
    public Vector2 dropOffset;
    public float randomDropRange;
    public int dropAmount;
    public int grouping = 1;

    protected virtual void Start() 
    {
        currentHealth = objectHealth;
    }

    public virtual void Hit(int damage, float breakDelay = 0f)
    {
        currentHealth -= damage;

        if (currentHealth > 0) { return; }

        StartCoroutine(Break(breakDelay));
    }

    protected IEnumerator Break(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if(dropItem != null)
        {
            DropItems(dropItem, dropAmount, dropOffset, grouping);
        }
        
        Destroy(this.gameObject);
    }

    protected void DropItems(ItemDataObject item, int amount, Vector2 offset, int grouping = 1)
    {
        int previousSign = 1;
        for (int i = 0; i < Mathf.FloorToInt(amount/grouping); i++)
        {
            Vector2 random = new Vector2(Random.Range(0.1f, randomDropRange), Random.Range(0.1f, randomDropRange));
            random *= previousSign == (int)Mathf.Sign(random.x/random.y) ? -1 : 1; 
            previousSign = (int)Mathf.Sign(random.x/random.y);

            Vector2 randomOffset = offset + random;

            GameObject droppedItem = Instantiate(worldItemPrefab, transform.position + (Vector3)randomOffset, Quaternion.identity);
            WorldItem  droppedItemData = droppedItem.GetComponent<WorldItem>();
            droppedItemData.InitializeItem(item, grouping + (i == 0 ? amount%grouping : 0));
        }
    }
}
