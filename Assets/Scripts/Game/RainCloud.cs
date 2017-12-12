using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCloud : Cloud, CastsShadow {

    public Transform[] leftRightEdges;

    public override void Initialize()
    {
        base.Initialize();
        GV.ws.shadowManager.RegisterShadow(this, transform);
    }

    public override void Refresh(float dt)
    {
        UpdateRain(dt);
        base.Refresh(dt);
    }

    public void UpdateRain(float dt)
    {

    }


    /////////////////////////////////////////////

    public Vector2[] RetrieveShadowEdges()
    {
        return new Vector2[] { leftRightEdges[0].transform.position, leftRightEdges[1].transform.position };
    }
}
