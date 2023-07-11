using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weather Command", menuName = "Developer Console/Commands/Weather")]
public class WeatherCommand : ConsoleCommand
{
    public override CommandReturn Process(string[] args)
    {
        if (args.Length != 1) 
        {
            return new CommandReturn(false, "Command 'weather' takes 1 argument");
        }

        WeatherState weatherOption;

        switch (args[0].ToLower())
        {
            case "reset":
                weatherOption = WeatherState.Clear;
            break;

            case "rain":
                weatherOption = WeatherState.Raining;
            break;

            case "storm":
                weatherOption = WeatherState.Storming;
            break;

            default:
                return new CommandReturn(false, "Invalid weather state");
        }
        
        WeatherManager.Instance.SetWeather(weatherOption);
        return new CommandReturn(true, $"Set weather to {weatherOption}");
    }
}
