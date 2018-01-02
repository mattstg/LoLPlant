using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAChangeFlow : TAEvent {

    CurrentState stateChange;
    public TAChangeFlow(CurrentState _stateChange) : base(TAEventType.Action)
    {
        stateChange = _stateChange;
    }

    public override void PerformEvent()
    {
        GV.ms.GoToNextFlow(stateChange);
    }
}
