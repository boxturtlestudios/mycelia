using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationControler : MonoBehaviour
{
    private static GameObject blackPanel;
    public static float fadeTime = 0.18f;
    public static float betweenFadesTime = 0.25f;

    void Start()
    {
        blackPanel = transform.GetChild(0).gameObject;
    }

    public static void BlackFade()
    {
        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.alphaCanvas(blackPanel.GetComponent<CanvasGroup>(), 1f, fadeTime)
            .setEase(LeanTweenType.easeOutQuad)); // Fade in

        sequence.append(betweenFadesTime); // Wait for specified duration

        sequence.append(LeanTween.alphaCanvas(blackPanel.GetComponent<CanvasGroup>(), 0f, fadeTime)
            .setEase(LeanTweenType.easeOutQuad)); // Fade out
    }
}
