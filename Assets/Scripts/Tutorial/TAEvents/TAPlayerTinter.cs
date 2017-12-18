using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAPlayerTinter : TAEvent {

    float tintLevel;

    public TAPlayerTinter(float _tintLevel) : base(TAEventType.Action)
    {
        tintLevel = _tintLevel;
    }

    public override void PerformEvent()
    {
        GV.ws.dnc.SetPlayerTint(tintLevel);
    }

}
