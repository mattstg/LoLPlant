using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAFreezeChar : TAEvent
{
    bool setFrozen;
    public TAFreezeChar(bool _setFrozen) : base(TAEventType.Action)
    {
        setFrozen = _setFrozen;
    }

    public override void PerformEvent()
    {
        (GV.ws.pc as TutorialPC).SetInputActive(!setFrozen, true);
    }
}
