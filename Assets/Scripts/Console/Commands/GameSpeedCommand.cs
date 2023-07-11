using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameSpeed Command", menuName = "Developer Console/Commands/GameSpeed")]
public class GameSpeedCommand : ConsoleCommand
{
    public override CommandReturn Process(string[] args)
    {
        if (args.Length != 1) 
        {
            return new CommandReturn(false, "Command 'gamespeed' takes 1 argument");
        }

        if (args[0].ToLower() == "reset")
        {
            TimeManager.Instance.SetTimeScale(1f);
            return new CommandReturn(true, "Reset game speed");
        }
    
        if (!float.TryParse(args[0], out float scale)) { return new CommandReturn(false, "Time scale must be a number"); } 
        if (!(scale <= 1000f && scale >= 0.01f)) { return new CommandReturn(false, "Time scale must be between 0.01 and 1000"); }
        
        TimeManager.Instance.SetTimeScale(scale);
        return new CommandReturn(true, $"Set game speed to {scale}");
    }
}
