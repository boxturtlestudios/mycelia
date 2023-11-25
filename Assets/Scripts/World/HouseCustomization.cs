using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCustomization : MonoBehaviour
{
    public GameObject growth;
    public GameObject roof;
    public GameObject pillars;
    public GameObject planks;
    public GameObject mailbox;
    public GameObject door;

    public House[] presets;
    public House currentHouse;

    private void Start() 
    {
        //StartCoroutine(CycleHouses());
        currentHouse.growthType = growth.GetComponent<SpriteRenderer>().sprite;
        currentHouse.roofType = roof.GetComponent<SpriteRenderer>().sprite;
        currentHouse.pillarColor = pillars.GetComponent<SpriteRenderer>().color;
        currentHouse.plankColor = planks.GetComponent<SpriteRenderer>().color;
        currentHouse.mailboxColor = mailbox.GetComponent<SpriteRenderer>().color;
        currentHouse.doorType = door.GetComponent<SpriteRenderer>().sprite;
    }

    private IEnumerator CycleHouses()
    {
        int index = 0;
        while(true)
        {
            SetHouse(presets[index%presets.Length]);
            index++;
            yield return new WaitForSeconds(1f);
        }
    }

    public void SetHouse(House house)
    {
        currentHouse = house;

        roof.GetComponent<SpriteRenderer>().sprite = house.roofType;
        pillars.GetComponent<SpriteRenderer>().color = house.pillarColor;
        planks.GetComponent<SpriteRenderer>().color = house.plankColor;
        mailbox.GetComponent<SpriteRenderer>().color = house.mailboxColor;
        growth.GetComponent<SpriteRenderer>().sprite = house.growthType;
        door.GetComponent<SpriteRenderer>().sprite = house.doorType;
    }
}

[System.Serializable]
public class House {
    public string name;
    public Sprite growthType;
    public Sprite roofType;
    public Color pillarColor;
    public Color plankColor;
    public Color mailboxColor;
    public Sprite doorType;
}
