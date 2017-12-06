using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TATimer : TAEvent
{
    float timer;
    string eventName;

    public TATimer(string _eventName, float _timer) : base(TAEventType.Action)
    {
        eventName = _eventName;
        timer = _timer;
    }

    public override void PerformEvent()
    {
        (GameObject.Instantiate(Resources.Load("Prefabs/TATimer")) as GameObject).GetComponent<TAEventTimer>().Initialize(timer,eventName);
    }
}
