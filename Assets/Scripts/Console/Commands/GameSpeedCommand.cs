using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameSpeed Command", menuName = "Developer Console/Commands/GameSpeed")]
public class GameSpeedCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        if (args.Length != 1) { Debug.Log("Command 'gamespeed' takes 1 argument"); return false; } //"Command "gamespeed" takes 1 argument"

        if (args[0].ToLower() == "reset")
        {
            TimeManager.Instance.SetTimeScale(1f);
            Debug.Log("Reset game speed");
            return true; //"Reset game speed"
        }
    
        if (!float.TryParse(args[0], out float scale)) { Debug.Log("Time scale must be a number"); return false; } //"Time scale must be a number"
        if (!(scale <= 1000f && scale >= 0.01f)) { Debug.Log("Time scale must be between 0.01 and 1000"); return false; } //"Time scale must be between 0.01 and 1000"
        TimeManager.Instance.SetTimeScale(scale);
        Debug.Log("Set game speed to " + scale);
        return true; //"Reset game speed to XXX"
    }
}
