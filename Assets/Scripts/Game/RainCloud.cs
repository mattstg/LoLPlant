using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCloud : Cloud {

    public override void Refresh(float dt)
    {
        UpdateRain(dt);
        base.Refresh(dt);
    }

    public void UpdateRain(float dt)
    {

    }
}
