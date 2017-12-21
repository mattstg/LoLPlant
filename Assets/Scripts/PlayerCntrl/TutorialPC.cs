using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPC : PlayerController {
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

    //public override void RaycastToSun() //Efficency of raycast to the sun
    //{
    //    //Need layer mask
    //    var layerMask = (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("Cloud"));
    //    //need distance
    //    Vector2 shadowAngle = GV.DegreeToVector2(GV.ws.dnc.groundToSunAngle);
    //    RaycastHit2D[] rayhits = Physics2D.RaycastAll(transform.position, shadowAngle, 30, layerMask);
    //    if (rayhits == null)
    //        GV.ws.plant.shadowCount = 0;
    //    else
    //        GV.ws.plant.shadowCount = rayhits.Length;
    //}

}
