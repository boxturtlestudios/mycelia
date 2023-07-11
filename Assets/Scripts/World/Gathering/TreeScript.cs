using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : ToolHittable
{
    public GameObject stumpPrefab;
    private Animator anim;

    protected override void Start()
    {
        base.Start();
        anim = gameObject.GetComponent<Animator>();
    }

    public override void Hit(int damage, float breakDelay)
    {
        currentHealth -= damage;
        if (currentHealth > 0) { return; }

        anim.SetTrigger("Falling"); //Contains event which calls OnTreeFallen();
        Instantiate(stumpPrefab, transform.position, Quaternion.identity, transform.parent);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void OnTreeFallen()
    {
        StartCoroutine(Break(0f));
    }
}
