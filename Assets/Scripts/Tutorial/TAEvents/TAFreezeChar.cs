using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAFreezeChar : TAEvent
{
    bool setFrozen;
    bool hardLock;
    public TAFreezeChar(bool _setFrozen, bool _hardLock = false) : base(TAEventType.Action)
    {
        setFrozen = _setFrozen;
        hardLock = _hardLock;
    }

    public override void PerformEvent()
    {
        (GV.ws.pc as TutorialPC).SetInputActive(!setFrozen, hardLock);
    }
}
