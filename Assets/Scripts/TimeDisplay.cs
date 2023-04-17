using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TimeFormat
{
    TwelveHourTime,
    TwentyfourHourTime
}

public class TimeDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;

    public TimeFormat timeFormat;
    private string currentTimeLabel;

    #region Event Subscription
    private void OnEnable() 
    {
        TimeManager.OnTimeChanged += UpdateTimeText;
    }
    private void OnDisable() 
    {
        TimeManager.OnTimeChanged -= UpdateTimeText;
    }
    #endregion

    private void UpdateTimeText()
    {
        int minute;
        int hour;

        if(timeFormat == TimeFormat.TwelveHourTime)
        {
            hour = TimeManager.Instance.Hour % 12;
            minute = TimeManager.Instance.Minute;
            if(hour == 0) {hour = 12;}

            currentTimeLabel = (TimeManager.Instance.Hour >= 12) ? " PM" : " AM";
        }
        else
        {
            hour = TimeManager.Instance.Hour;
            minute = TimeManager.Instance.Minute;
            currentTimeLabel = "";
        }

        if (minute % 10 != 0) {return;}

        timeText.text = $"{hour}:{minute:00}{currentTimeLabel}";
    }
}
