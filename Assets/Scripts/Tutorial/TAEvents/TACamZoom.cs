using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TACamZoom : TAEvent {

    float zoomAmt;

    public TACamZoom(float _zoomAmt) : base(TAEventType.Action)
    {
        zoomAmt = _zoomAmt;
    }

    public override void PerformEvent()
    {
        Camera.main.orthographicSize = zoomAmt;
    }
}
