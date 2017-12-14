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
}
