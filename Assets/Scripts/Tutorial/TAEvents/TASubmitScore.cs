﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASubmitScore : TAEvent {

    public TASubmitScore() : base(TAEventType.Action)
    {

    }

    public override void PerformEvent()
    {
        ProgressTracker.Instance.SetScore(GV.ws.plant.heightInt);
    }

}
