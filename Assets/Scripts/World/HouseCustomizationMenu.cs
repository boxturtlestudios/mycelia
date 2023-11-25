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
        UnityEngine.ColorUtility.TryParseHtmlString("#" + wallColorHex, out Color wallColor);
        currentHouse.plankColor = wallColor;
        houseCustomization.SetHouse(currentHouse);
        SoundManager.Instance.Play("Button");
    }

    public void SetPillars(string pillarColorHex)
    {
        UnityEngine.ColorUtility.TryParseHtmlString("#" + pillarColorHex, out Color pillarColor);
        currentHouse.pillarColor = pillarColor;
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
        UnityEngine.ColorUtility.TryParseHtmlString("#" + mailboxColorHex, out Color mailboxColor);
        currentHouse.mailboxColor = mailboxColor;
        houseCustomization.SetHouse(currentHouse);
        SoundManager.Instance.Play("Button");
    }

    public void SetDoor(Sprite doorOption)
    {
        currentHouse.doorType = doorOption;
        houseCustomization.SetHouse(currentHouse);
        SoundManager.Instance.Play("Button");
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
    }
}
