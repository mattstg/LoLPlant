using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TATimer : TAEvent
{
    float timer;

    public TATimer(TAEventType _eventType, string _eventName, float _timer) : base(_eventType, _eventName)
    {

    }

    public override void PerformEvent()
    {
        (GameObject.Instantiate(Resources.Load("Prefabs/TATimer")) as GameObject).GetComponent<TAEventTimer>().Initialize(timer,eventName);
    }
}
