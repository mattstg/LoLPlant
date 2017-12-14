using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLab : Plant
{
    float waterTriggerThreshold = .5f;
    float sunTriggerThreshold = .95f;

    public override void Refresh(float dt)
    {
        Debug.Log("sun: " + sun);
        if (water >= waterTriggerThreshold)
        {
            Debug.Log("water threshold sent");
            TAEventManager.Instance.ReceiveActionTrigger("Water");
        }
        if (sun >= sunTriggerThreshold)
            TAEventManager.Instance.ReceiveActionTrigger("Sun");

        base.Refresh(dt);
    }

}
