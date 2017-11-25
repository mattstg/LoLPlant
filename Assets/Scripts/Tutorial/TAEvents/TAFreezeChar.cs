using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAFreezeChar : TAEvent
{
    bool setFrozen;
    public TAFreezeChar(TAEventType _eventType, string _eventName, bool _setFrozen) : base(_eventType, _eventName)
    {
        setFrozen = _setFrozen;
    }

    public override void PerformEvent()
    {
        (GV.ws.pc as TutorialPC).SetCharActive(setFrozen);
    }
}
