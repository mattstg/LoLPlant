using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASetDNC : TAEvent
{
    float hour;
    int day;
    bool clockActive;

    public TASetDNC(bool _clockActive, float _hour = -1, int _day = -1) : base(TAEventType.Action)
    {
        clockActive = _clockActive;
        hour = _hour;
        day = _day;
    }

    public override void PerformEvent()
    {
        GV.ws.dnc.UpdateTimeData();
        if (hour >= 0)
        {
            if (day < 0)
                day = GV.ws.dnc.day;
            GV.ws.dnc.SetTime(day, hour);
        }
        GV.ws.dnc.SetClockActive(clockActive);        
    }
}
