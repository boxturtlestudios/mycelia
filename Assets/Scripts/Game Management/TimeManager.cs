using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public uint minuteTick { get; private set;}
    public int Minute { get; private set;}
    public int Hour { get; private set;}
    public int Day { get; private set;}

    [SerializeField]
    private int gameStartTime;
    [SerializeField]
    private float minutesPerSecond = 1f;
    private float timer = 0;


    //Debug
    static bool timeSpedUp = false;

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

            Day = Mathf.FloorToInt((minuteTick/60)/24);
            Hour = Mathf.FloorToInt(minuteTick/60)%24;
            Minute = (int)(minuteTick%60);

            if(OnTimeChanged != null)
            {
                OnTimeChanged();
            }
            timer = 1/minutesPerSecond;
        }
    }

   public void SpeedUpTime()
   {
        timeSpedUp = !timeSpedUp;
            
        minutesPerSecond = timeSpedUp ? 1000f : 1f;
   }

}
