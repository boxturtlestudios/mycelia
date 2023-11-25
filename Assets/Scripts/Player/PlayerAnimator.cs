using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionType
{
    Movement,
    Facing
}

public enum PlayerState
{
    Walking,
    UsingTool,
    Interacting,
    Busy
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 direction;
    public PlayerState playerState;

    private void Start() 
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDirection(Vector2 dir, DirectionType type)
    {
        if(type == DirectionType.Movement || playerState == PlayerState.Walking)
        {
            direction = dir;
        }
        else if (type == DirectionType.Facing || playerState != PlayerState.Walking)
        {
            direction = dir;
            UpdateAnimator();
        }
    }

    public void Trigger(string name)
    {
        if(name == "Dig" || name == "Hoe" || name == "Axe" || name == "Pickaxe")
        {
            if(playerState == PlayerState.UsingTool)
            {
                animator.SetTrigger(name);
            }
        }
    }


    public void UpdateAnimator()
    {
        if (playerState == PlayerState.Walking)
        {
            animator.SetFloat("Speed", direction.sqrMagnitude);

            if(direction != Vector2.zero)
            {
                animator.SetFloat("Vertical", direction.y);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            animator.SetFloat("Vertical", direction.y);
        }

        if(!spriteRenderer.flipX && direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void Update() 
    {
        if(playerState == PlayerState.Walking)
        {
            UpdateAnimator();
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction);
    }
}
