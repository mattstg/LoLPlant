using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPC : PlayerController {

    bool inTheSun = false;
    float timeInSunToSendTrigger = 1.5f;

    public override void Refresh(float dt)
    {
        if(inTheSun)
        { 
            timeInSunToSendTrigger -= dt;
            if (timeInSunToSendTrigger <= 0)
                TAEventManager.Instance.ReceiveActionTrigger("Sun");
        }
        else
        {
            timeInSunToSendTrigger = 1.5f;
        }
        base.Refresh(dt);
    }

    public override void OnCollisionEnter2D(Collision2D coli)
    {
        switch (coli.gameObject.tag)
        {
            case "Aphid":
                TAEventManager.Instance.ReceiveActionTrigger("Aphid");
                break;
        }
        base.OnCollisionEnter2D(coli);
    }

    public override void RaycastToSun() //Efficency of raycast to the sun
    {
        //Need layer mask
        var layerMask = (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("Cloud") | (1 << LayerMask.NameToLayer("Awning")));
        //need distance
        Vector2 shadowAngle = GV.DegreeToVector2(GV.ws.dnc.groundToSunAngle);
        RaycastHit2D[] rayhits = Physics2D.RaycastAll(transform.position, shadowAngle, 30, layerMask);

        string hitsName = "";
        foreach (RaycastHit2D rc in rayhits)
            hitsName += rc.transform.name;


        if (rayhits == null || rayhits.Length <= 0)
        {
            GV.ws.plant.shadowCount = 0;
            inTheSun = true;
        }
        else
        {
            GV.ws.plant.shadowCount = 1;
            inTheSun = false;
        }
    }

}
