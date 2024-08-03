using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : ItemInteractable
{
    public override void Interact()
    {
        base.Interact();
        Uninteract();
    }
}
