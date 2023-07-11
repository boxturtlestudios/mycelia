using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    private MyceliaInputActions inputActions;
    private InputAction interact;

    public Vector2 bubbleOffset;

    private bool inRange = false;
    private bool isSelected = false;
    

   #region Initialize Inputs

    private void Awake() {
        inputActions = new MyceliaInputActions();
    }

    private void OnEnable() {
        interact = inputActions.Player.Interact;
        interact.Enable();
        interact.performed += (InputAction.CallbackContext context) => { if(inRange && !isSelected) Interact(); };
    }

    private void OnDisable() {
        inputActions.Disable();
    }

   #endregion

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            inRange = true;
            InteractBubbleSystem.Show(transform.position + (Vector3)bubbleOffset, "X");
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            inRange = false;
            InteractBubbleSystem.Hide();
            Disengage();
        }
    }

    protected virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

    protected virtual void Uninteract()
    {
        Debug.Log("Uninteracted with " + gameObject.name);
    }

    protected virtual void Disengage()
    {
        Debug.Log("Disengaged " + gameObject.name);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)bubbleOffset, 0.1f);
    }

}
