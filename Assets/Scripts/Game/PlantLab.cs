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
            TAEventManager.Instance.RecieveActionTrigger("Water");
        if (sun >= sunTriggerThreshold)
            TAEventManager.Instance.RecieveActionTrigger("Sun");

        base.Refresh(dt);
    }

}
