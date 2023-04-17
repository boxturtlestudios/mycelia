using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : TooltipTriggerBase
{
    public string header;
    [TextArea(5, 20)]
    public string content;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(content, header);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }
}
