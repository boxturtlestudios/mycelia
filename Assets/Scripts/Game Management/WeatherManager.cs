using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum WeatherState
{
    Clear,
    Raining,
    Storming
}

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance { get; private set; }
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogAssertion("Duplicate WeatherManager destroyed!");
            Destroy(gameObject);
        }
    }


    private Light2D globalLight;
    [VectorLabels("Min", "Max")]
    public Vector2 lightLevels;
    public Color dayColor;
    public Color nightColor;

    public WeatherState currentWeatherState;
    private WeatherState previousWeatherState;
    public float rainLightLevelChange;
    public float stormLightLevelChange;
    public Color rainColor;
    public Color stormColor;
    [Range(0f, 1f)] public float rainColorWeight = 0.5f;
    [Range(0f, 1f)] public float stormColorWeight = 0.5f;

    public GameObject rainGenerators;

    #region Event Subscription
    private void OnEnable() 
    {
        TimeManager.OnTimeChanged += UpdateWeather;
    }
    private void OnDisable() 
    {
        TimeManager.OnTimeChanged -= UpdateWeather;
    }
    #endregion

    private void Start() 
    {
        SoundManager.Instance.Play("Ambient 1");

        globalLight = GameObject.FindGameObjectWithTag("Global Light").GetComponent<Light2D>();
        if(currentWeatherState == WeatherState.Clear)
        {
            SoundManager.Instance.Play("Birds");
        }
        //rainGenerators = GameObject.FindGameObjectWithTag("Rain Generators");
    }

    private void UpdateSkyColor()
    {
        uint minuteTick = TimeManager.Instance.minuteTick;

        float timeOfDay = minuteTick%(60*24);
        float percentOfDay = timeOfDay/(24*60);

        float timeLightLevel = (lightLevels.y - lightLevels.x) * Mathf.Sin(Mathf.Deg2Rad * (percentOfDay*180)) + lightLevels.x;
        Color timeColor = Color.Lerp(nightColor, dayColor, Mathf.Sin(Mathf.Deg2Rad * (percentOfDay*180)));

        float lightLevel;
        Color skyColor;
        switch (currentWeatherState)
        {
            case WeatherState.Clear:
                lightLevel = timeLightLevel;
                skyColor = timeColor;
                break;

            case WeatherState.Raining:
                lightLevel = Mathf.Clamp(timeLightLevel + rainLightLevelChange, lightLevels.x, lightLevels.y);
                skyColor = Color.Lerp(timeColor, rainColor, rainColorWeight);
                break;

            case WeatherState.Storming:
                lightLevel = Mathf.Clamp(timeLightLevel + stormLightLevelChange, lightLevels.x, lightLevels.y);
                skyColor = Color.Lerp(timeColor, stormColor, stormColorWeight);
                break;

            default:
                lightLevel = timeLightLevel;
                skyColor = timeColor;
                break;
        }

        globalLight.intensity = lightLevel;
        globalLight.color = skyColor;
    }

    private void UpdateWeather()
    {
        if(currentWeatherState != previousWeatherState)
        {
            if(previousWeatherState == WeatherState.Clear && currentWeatherState == WeatherState.Raining)
            {
                Debug.Log("Changing from clear to rain");
                rainGenerators.SetActive(true);
                SoundManager.Instance.Stop("Birds", 0.01f);
                SoundManager.Instance.Play("LightRain", 0.01f);
            }
            else if (previousWeatherState == WeatherState.Raining && currentWeatherState == WeatherState.Clear)
            {
                Debug.Log("Changing from rain to clear");
                rainGenerators.SetActive(false);
                SoundManager.Instance.Stop("LightRain", 0.01f);
                SoundManager.Instance.Play("Birds", 0.01f);                
            }
            else if (previousWeatherState == WeatherState.Raining && currentWeatherState == WeatherState.Storming)
            {
                Debug.Log("Changing from rain to storm");
                SoundManager.Instance.Stop("LightRain", 0.01f);
                SoundManager.Instance.Play("Storm", 0.01f);     
            }
            else if (previousWeatherState == WeatherState.Clear && currentWeatherState == WeatherState.Storming)
            {
                Debug.Log("Changing from clear to storm");
                rainGenerators.SetActive(true);
                SoundManager.Instance.Stop("Birds", 0.01f);
                SoundManager.Instance.Play("Storm", 0.01f);     
            }
            else if (previousWeatherState == WeatherState.Storming && currentWeatherState == WeatherState.Clear)
            {
                Debug.Log("Changing from storm to clear");
                rainGenerators.SetActive(false);
                SoundManager.Instance.Stop("Storm", 0.01f);
                SoundManager.Instance.Play("Birds", 0.01f);     
            }
            else if (previousWeatherState == WeatherState.Storming && currentWeatherState == WeatherState.Raining)
            {
                Debug.Log("Changing from storm to rain");
                SoundManager.Instance.Stop("Storm", 0.01f);
                SoundManager.Instance.Play("LightRain", 0.01f);     
            }
        }

        UpdateSkyColor();

        previousWeatherState = currentWeatherState;
    }

    public void SetWeather(WeatherState weatherState)
    {
        currentWeatherState = weatherState;
    }

    /*private IEnumerator FadeWeatherStates(float fadeRate)
    {
        float progress = 0;
        while(progress < 1)
        {
            

            progress += fadeRate;
            yield return new WaitForSeconds(0.01f);
        }
    }*/
}
