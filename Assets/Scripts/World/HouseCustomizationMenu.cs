using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using BoxTurtleStudios.Utilities;
using Unity.VisualScripting;

public class HouseCustomizationMenu : MonoBehaviour
{
    public HouseCustomization houseCustomization;
    public House previousHouse;
    public House currentHouse;

    public Sprite selectedTab;
    public Sprite regularTab;

    public void StartCustomization()
    {
        Debug.Log("Starting Customization");
        currentHouse = houseCustomization.currentHouse;
        previousHouse = houseCustomization.currentHouse.CloneViaFakeSerialization();

        SoundManager.Instance.Play("Open Menu");
        ResetMenus();
        ResetMenus();
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = selectedTab;
    }

    public void EndCustomization(bool saved)
    {
        if(saved)
        {
            Debug.Log("Saving current house");
            houseCustomization.SetHouse(currentHouse);
        }
        else
        {
            Debug.Log("Reverting to previous house");
            houseCustomization.SetHouse(previousHouse);
        }

        SoundManager.Instance.Play("Close Menu");
        UIControl.Instance.CloseHouseCustomization();
    }


    public void OpenMenu(int index)
    {
        ResetMenus();
        transform.GetChild(0).GetChild(index).gameObject.SetActive(true);
        transform.GetChild(1).GetChild(index).GetComponent<UnityEngine.UI.Image>().sprite = selectedTab;
        SoundManager.Instance.Play("Tab Switch");
    }

    public void SetRoof(Sprite roofOption)
    {
        currentHouse.roofType = roofOption;
        houseCustomization.SetHouse(currentHouse);
        SoundManager.Instance.Play("Button");
    }

    public void SetWalls(string wallColorHex)
    {
        currentHouse.plankColor = HexToColor(wallColorHex);
        houseCustomization.SetHouse(currentHouse);
        SoundManager.Instance.Play("Button");
    }

    public void SetPillars(string pillarColorHex)
    {
        currentHouse.pillarColor = HexToColor(pillarColorHex);
        houseCustomization.SetHouse(currentHouse);
        SoundManager.Instance.Play("Button");
    }

    public void SetGrowth(Sprite growthOption)
    {
        currentHouse.growthType = growthOption;
        houseCustomization.SetHouse(currentHouse);
        SoundManager.Instance.Play("Button");
    }

    public void SetMailbox(string mailboxColorHex)
    {
        currentHouse.mailboxColor = HexToColor(mailboxColorHex);
        houseCustomization.SetHouse(currentHouse);
        SoundManager.Instance.Play("Button");
    }

    public void SetDoor(Sprite doorOption)
    {
        currentHouse.doorType = doorOption;
        houseCustomization.SetHouse(currentHouse);
        SoundManager.Instance.Play("Button");
    }

    Color HexToColor(string hex)
    {
        UnityEngine.ColorUtility.TryParseHtmlString("#" + hex, out Color color);
        return color;
    }

    void ResetMenus()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            transform.GetChild(1).GetChild(i).GetComponent<UnityEngine.UI.Image>().sprite = regularTab;
        }

        SetSelections();
    }

    public void SetSelections()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).childCount; j++)
            {
                GameObject currentOption = transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetChild(j).gameObject;
                currentOption.transform.GetChild(0).gameObject.SetActive(false);

                HouseOptionButton currentButton = currentOption.GetComponent<HouseOptionButton>();
                if (currentButton == null) { continue; }
                switch (currentButton.optionType)
                {
                    case HouseOptionButton.OptionType.Roof:
                        if (currentButton.sprite == currentHouse.roofType) { currentOption.transform.GetChild(0).gameObject.SetActive(true); }
                        break;
                    case HouseOptionButton.OptionType.Walls:
                        if (HexToColor(currentButton.color) == currentHouse.plankColor) { currentOption.transform.GetChild(0).gameObject.SetActive(true); }
                        break;
                    case HouseOptionButton.OptionType.Pillars:
                        if (HexToColor(currentButton.color) == currentHouse.pillarColor) { currentOption.transform.GetChild(0).gameObject.SetActive(true); }
                        break;
                    case HouseOptionButton.OptionType.Growth:
                        if (currentButton.sprite == currentHouse.growthType) { currentOption.transform.GetChild(0).gameObject.SetActive(true); }
                        break;
                    case HouseOptionButton.OptionType.Door:
                        if (currentButton.sprite == currentHouse.doorType) { currentOption.transform.GetChild(0).gameObject.SetActive(true); }
                        break;
                    case HouseOptionButton.OptionType.Mailbox:
                        if (HexToColor(currentButton.color) == currentHouse.mailboxColor) { currentOption.transform.GetChild(0).gameObject.SetActive(true); }
                        break;
                    default:
                        Debug.LogError("Unknown house option type.");
                        break;
                }
            }
        }
    }
}
