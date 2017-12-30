using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPC : PlayerController {

    int collidedWithAphid = 0; //If collide enough times disable tutorial aphids
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

    public void OnTriggerEnter2D(Collider2D coli)
    {
        switch (coli.gameObject.tag)
        {
            case "Aphid":
                TAEventManager.Instance.ReceiveActionTrigger("Aphid");
                collidedWithAphid++;
                Debug.Log("coli with aphid");
                if(collidedWithAphid > 8)
                    GV.ws.aphidManager.SetAphidsActive(false); //Turn off aphids if hit them 8 times in tutorial
                break;
        }
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
