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

    public HouseColor[] colors;
}

[System.Serializable]
public class HouseColor {
    public string name;
    public Color color;
}
