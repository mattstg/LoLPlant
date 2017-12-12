using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCloud : Cloud, CastsShadow {

    public Transform[] leftRightEdges;

    public override void Initialize()
    {
        base.Initialize();
        GV.ws.shadowManager.RegisterShadow(this, false, transform);
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

    public void RegisterShadow(bool _isStatic)
    {
        //do nothing atm
    }

    public Vector2[] RetrieveShadowEdges()
    {
        Debug.Log("Cloud Transform: " + transform.position + ", edges(L/R): " + leftRightEdges[0].transform.position + " ,,, " + leftRightEdges[1].transform.position);
        return new Vector2[] { leftRightEdges[0].transform.position, leftRightEdges[1].transform.position };
    }
}
