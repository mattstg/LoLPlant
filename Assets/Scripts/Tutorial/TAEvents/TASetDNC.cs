using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASetDNC : TAEvent
{
    float timer;
    string eventName;

    public TASetDNC(bool clockActive, float hour = -1, int day = -1) : base(TAEventType.Action)
    {
        if (hour >= 0)
        {
            if (day < 0)
                day = GV.ws.dnc.day;
            GV.ws.dnc.SetTime(day, hour);
        }
        GV.ws.dnc.SetClockActive(clockActive);
    }

    public override void PerformEvent()
    {
        (GameObject.Instantiate(Resources.Load("Prefabs/TATimer")) as GameObject).GetComponent<TAEventTimer>().Initialize(timer,eventName);
    }
}
