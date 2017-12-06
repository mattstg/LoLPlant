using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLab : Plant
{
    float waterTriggerThreshold;
    float sunTriggerThreshold;

    public override void Refresh(float dt)
    {
        if (water >= waterTriggerThreshold)
            TAEventManager.Instance.ReceiveActionTrigger("Water");
        if (sun >= sunTriggerThreshold)
            TAEventManager.Instance.ReceiveActionTrigger("Sun");

        base.Refresh(dt);
    }

}
