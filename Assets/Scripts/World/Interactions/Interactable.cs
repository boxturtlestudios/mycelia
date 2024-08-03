using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public Vector2 bubbleOffset;

    protected PlayerMovement playerMovement;
    protected PlayerActions playerActions;
    

   private void Start() 
   {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
   }
   
    public virtual void Interact()
    {
        if(playerMovement.playerState == PlayerState.Interacting || playerMovement.playerState == PlayerState.Busy || playerMovement.playerState == PlayerState.UsingTool) { return; }
        playerMovement.playerState = PlayerState.Interacting;
        Debug.Log("Interacted with " + gameObject.name);
    }

    public virtual void Uninteract()
    {
        playerMovement.playerState = PlayerState.Walking;
        Debug.Log("Uninteracted with " + gameObject.name);
    }

    public virtual void Disengage()
    {
        playerMovement.playerState = PlayerState.Walking;
        Debug.Log("Disengaged " + gameObject.name);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)bubbleOffset, 0.1f);
    }

}
