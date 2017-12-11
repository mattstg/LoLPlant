using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCloud : RainCloud
{
    public override void Refresh(float dt)
    {
        UpdateRain(dt);
        //base.Refresh(dt);
    }

}
