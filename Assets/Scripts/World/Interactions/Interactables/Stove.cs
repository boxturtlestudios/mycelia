using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : Interactable
{
    public override void Interact()
    {
        base.Interact();
        Uninteract();
    }
}
