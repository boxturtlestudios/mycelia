using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInteractable : Interactable
{
    public ItemDataObject[] acceptableItems;

    public override void Interact()
    {
        if (acceptableItems.Contains(playerActions.currentItem))
        {
            base.Interact();
        }
    }

}
