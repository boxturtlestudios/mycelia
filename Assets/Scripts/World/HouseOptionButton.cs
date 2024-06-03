using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HouseOptionButton : MonoBehaviour, IPointerClickHandler
{
    public enum OptionType {
        Roof,
        Walls,
        Pillars,
        Growth,
        Door,
        Mailbox
    }

    public OptionType optionType;
    public HouseCustomizationMenu houseCustomizationMenu;
    public Sprite sprite;
    public string color;
    /*
    public string color
    {
        get { return color; }
        set
        {
            color = value;
            if (optionType == OptionType.Walls || optionType == OptionType.Pillars || optionType == OptionType.Mailbox)
            {
                UnityEngine.ColorUtility.TryParseHtmlString("#" + color, out Color colorObject);
                GetComponent<SpriteRenderer>().color = colorObject;
            }
        }
    }
    */

    private void OnEnable()
    {
        Debug.Log(transform.parent.parent.parent.parent.parent);
        houseCustomizationMenu = transform.parent.parent.parent.parent.parent.GetComponent<HouseCustomizationMenu>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (optionType)
        {
            case OptionType.Roof:
                houseCustomizationMenu.SetRoof(sprite);
                break;
            case OptionType.Walls:
                houseCustomizationMenu.SetWalls(color);
                break;
            case OptionType.Pillars:
                houseCustomizationMenu.SetPillars(color);
                break;
            case OptionType.Growth:
                houseCustomizationMenu.SetGrowth(sprite);
                break;
            case OptionType.Door:
                houseCustomizationMenu.SetDoor(sprite);
                break;
            case OptionType.Mailbox:
                houseCustomizationMenu.SetMailbox(color);
                break;
            default:
                Debug.LogError("Unknown option type.");
                break;
        }

        houseCustomizationMenu.SetSelections();
    }
}
