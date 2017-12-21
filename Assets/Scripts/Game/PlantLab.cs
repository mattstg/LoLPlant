using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLab : Plant
{
    float waterTriggerThreshold = .5f;

    public override void Refresh(float dt)
    {
        base.Refresh(dt);

        //Debug.Log("sun: " + sun);
        if (water >= waterTriggerThreshold)
        {
            //Debug.Log("water threshold sent");
            TAEventManager.Instance.ReceiveActionTrigger("Water");
        }
    }

}
