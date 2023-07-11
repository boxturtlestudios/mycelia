using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThrough : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float transparentOpacity = 0.5f;
    [SerializeField] private float fadeRate = 0.1f;

    private Color opaque = new Color(1, 1, 1, 1);

    private void Start() 
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            StopCoroutine("Fade");
            StartCoroutine(Fade(spriteRenderer.color, new Color(1, 1, 1, transparentOpacity), fadeRate));
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            StopCoroutine("Fade");
            StartCoroutine(Fade(spriteRenderer.color, opaque, fadeRate));
        }
    }

    private IEnumerator Fade(Color start, Color end, float fadeRate)
    {
        float t = 0;
        while(t < 1)
        {
            spriteRenderer.color = Color.Lerp(start, end, t);
            if(transform.childCount > 0)
            {
                for(int i = 0; i < transform.childCount-1; i++)
                {
                    transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = spriteRenderer.color;
                }
            }
            yield return new WaitForSeconds(0.01f);
            t += fadeRate;
        }
    }
}
