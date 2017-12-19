using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCloud : RainCloud
{
    public override void Initialize(bool singleScreenLoad)
    {
        //Single screen load is assumed for tutorial cloud
        altitudeRange = GV.raincloudAltitudeRange;
        travelRange = GV.raincloudTravelRange;
        maxDropsPerFrame = GV.maxDropsPerFrame;

        rainRate = 15;
        raindrop = GV.ws.raincloudManager.raindrop;
        
        GV.ws.shadowManager.RegisterShadow(this, transform);
    }

    public override void Refresh(float dt)
    {
        Rain();
    }

}
