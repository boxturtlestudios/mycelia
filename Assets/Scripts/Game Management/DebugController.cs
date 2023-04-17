using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    public KeyCode timeSpeedKey;
    public KeyCode toggleRainKey;

    void Update()
    {
        if(Input.GetKeyDown(timeSpeedKey))
        {
            TimeManager.Instance.SpeedUpTime();
        }

        if (Input.GetKeyDown(toggleRainKey))
        {
            if(WeatherManager.Instance.currentWeatherState != WeatherState.Raining)
            {
                WeatherManager.Instance.currentWeatherState = WeatherState.Raining;
            }
            else
            {
                WeatherManager.Instance.currentWeatherState = WeatherState.Clear;
            }
        }
    }
}
