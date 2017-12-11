using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASetDNC : TAEvent
{
    float timer;
    string eventName;

    public TASetDNC(bool freeze, float setTime = -1) : base(TAEventType.Action)
    {
        if (setTime >= 0)
            GV.ws.dnc.SetTime(setTime);
        GV.SetClockStopped(freeze);
    }

    public override void PerformEvent()
    {
        (GameObject.Instantiate(Resources.Load("Prefabs/TATimer")) as GameObject).GetComponent<TAEventTimer>().Initialize(timer,eventName);
    }
}
