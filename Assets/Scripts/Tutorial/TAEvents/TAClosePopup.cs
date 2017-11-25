using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAClosePopup : TAEvent
{
    public TAClosePopup(TAEventType _eventType, string _eventName) : base(_eventType, _eventName)
    {
        
    }

    public override void PerformEvent()
    {
        GV.ws.popupManager.ClosePopup();
    }
}
