using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : Interactable
{

    private bool currentlyLit = false;

    public override void Interact()
    {
        base.Interact();
        transform.GetChild(1).gameObject.SetActive(!currentlyLit);
        transform.GetChild(2).gameObject.SetActive(!currentlyLit);
        currentlyLit = !currentlyLit;
        Uninteract();
    }

}
