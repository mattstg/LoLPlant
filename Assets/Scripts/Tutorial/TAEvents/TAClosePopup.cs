using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAClosePopup : TAEvent
{
    public TAClosePopup() : base(TAEventType.Action)
    {
        
    }

    public override void PerformEvent()
    {
        GV.ws.popupManager.ClosePopup();
    }
}
