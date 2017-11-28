﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TAEvent
{
    public enum TAEventType { Trigger, Action }
    public TAEventType eventType;
    
    public TAEvent(TAEventType _eventType)
    {
        eventType = _eventType;
    }

    public virtual void PerformEvent()
    {

    }

}
