using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TADayEndPanel : TAEvent
{

    public TADayEndPanel() : base(TAEventType.Action)
    {

    }

    public override void PerformEvent()
    {
        GV.ws.dayEndManager.DayEnd();
    }
}
