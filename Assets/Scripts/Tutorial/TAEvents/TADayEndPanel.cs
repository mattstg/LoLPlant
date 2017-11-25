using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TADayEndPanel : TAEvent
{

    public TADayEndPanel(TAEventType _eventType, string _eventName) : base(_eventType, _eventName)
    {

    }

    public override void PerformEvent()
    {
        GV.ws.dayEndManager.DayEnd();
    }
}
