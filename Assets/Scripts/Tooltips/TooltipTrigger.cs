using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : TooltipTriggerBase
{
    public float hoverDelay = 0.8f;
    public string header;
    [TextArea(5, 20)]
    public string content;

    private Coroutine lastCoroutine = null;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (lastCoroutine != null) { StopCoroutine(lastCoroutine); }
        lastCoroutine = StartCoroutine(DelayedShow(content, header, hoverDelay));
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
        StopCoroutine(lastCoroutine);
    }

    private IEnumerator DelayedShow(string _content, string _header, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        TooltipSystem.Show(content, header);
    }
}
