using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mailbox : Interactable
{
    public RadialMenu menu;

    private void OnValidate() 
    {
        foreach(RadialOption i in menu.options)
        {
            i.function = this.GetType().GetMethod(i.functionName);
            i.obj = this;
        }
    }

    public override void Interact()
    {
        base.Interact();
        RadialMenuSystem.Show(transform.position + (Vector3)bubbleOffset, menu);
        InteractBubbleSystem.Hide();
    }

    public override void Uninteract()
    {
        base.Uninteract();
        RadialMenuSystem.Hide();
        InteractBubbleSystem.Show(transform.position + (Vector3)bubbleOffset, "X");
    }

    public override void Disengage()
    {
        base.Disengage();
        RadialMenuSystem.Hide();
    }

    public void CheckMail()
    {
        Debug.Log("Checking Mail");
    }

    public void CustomizeHouse()
    {
        UIControl.Instance.OpenHouseCustomization();
        RadialMenuSystem.Hide();
    }

    public void MoveHouse()
    {
        Debug.Log("Moving House");
    }
}
