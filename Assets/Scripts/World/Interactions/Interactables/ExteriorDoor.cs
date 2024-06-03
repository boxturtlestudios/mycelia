using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExteriorDoor : Interactable
{

    public Transform interiorSpawn;

    public override void Interact()
    {
        base.Interact();
        StartCoroutine(EnterHouse());
        Uninteract();
    }

    private IEnumerator EnterHouse()
    {
        UIAnimationControler.BlackFade();
        yield return new WaitForSeconds(0.16f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = interiorSpawn.position;
        CameraManager.Instance.ViewHouseInterior();
    }
}
