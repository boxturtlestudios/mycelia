using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public enum Seasons 
{
    Spring = 0,
    Summer = 1,
    Fall = 2,
    Winter = 3
}

[DisallowMultipleComponent]
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogAssertion("Duplicate TimeManager destroyed!");
            Destroy(gameObject);
        }
    }

    public delegate void ChangeTime();
    public static event ChangeTime OnTimeChanged;

    public uint minuteTick { get; private set; }
    public int Minute { get; private set; }
    public int Hour { get; private set; }
    public int Day { get; private set; }
    public Seasons Season; //{ get; private set; }
    public int Year { get; private set; }

    [SerializeField]
    private int gameStartTime;
    [SerializeField]
    private float minutesPerSecond = 1f;
    private float timer = 0;

    [SerializeField]
    private Material seasonsMat;

    private void Start()
    {
        minuteTick = (uint)(60*gameStartTime);
    }

    private void Update() 
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            minuteTick++;

            Year = Mathf.FloorToInt((minuteTick/60)/24/28/4);
            //Season = (Seasons)(Mathf.FloorToInt((minuteTick/60)/24/28)%4);
            Day = Mathf.FloorToInt((minuteTick/60)/24)%28;
            Hour = Mathf.FloorToInt(minuteTick/60)%24;
            Minute = (int)(minuteTick%60);

            UpdateSeason();

            if(OnTimeChanged != null)
            {
                OnTimeChanged();
            }
            timer = 1/minutesPerSecond;
        }
    }

    [Button]
    void UpdateSeason()
    {
        seasonsMat.SetFloat("_Season", (int)Season);
    }

   public void SetTimeScale(float scale)
   {
        minutesPerSecond = scale;
   }

}
