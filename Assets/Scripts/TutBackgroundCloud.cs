using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutBackgroundCloud : Cloud {

    public override void Initialize(bool singleLockedScreen)
    {
        base.Initialize(singleLockedScreen);
        altitudeRange = GV.tutCloudAltitudeRange;
        altitude = Random.Range(altitudeRange.x, altitudeRange.y);
        this.transform.Translate(0, altitude - this.transform.localPosition.y, 0);
    }
}
