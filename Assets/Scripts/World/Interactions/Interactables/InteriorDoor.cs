using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteriorDoor : Interactable
{

    public Transform exteriorSpawn;

    public override void Interact()
    {
        base.Interact();
        StartCoroutine(ExitHouse());
        Uninteract();
    }

    private IEnumerator ExitHouse()
    {
        UIAnimationControler.BlackFade();
        yield return new WaitForSeconds(0.16f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = exteriorSpawn.position;
    }
}
