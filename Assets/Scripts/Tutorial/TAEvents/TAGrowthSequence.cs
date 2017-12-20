using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAGrowthSequence : TAEvent {

    bool enable;

    public TAGrowthSequence(bool _enable) : base(TAEventType.Action)
    {
        enable = _enable;
    }

    public override void PerformEvent()
    {
        GV.ws.dnc.BeginGrowing();
        GV.ws.plant.BeginGrowing();
    }
}
