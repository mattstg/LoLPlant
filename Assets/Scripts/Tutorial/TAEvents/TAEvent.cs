using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TAEvent
{
    public enum TAEventType { Trigger, Action }
    public TAEventType eventType;
    public string eventName;
    
    public TAEvent(TAEventType _eventType, string _eventName)
    {
        eventName = _eventName;
        eventType = _eventType;
    }

    public virtual void PerformEvent()
    {

    }

}
