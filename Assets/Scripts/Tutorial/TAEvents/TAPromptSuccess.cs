using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAPromptSuccess : TAEvent {

    public TAPromptSuccess() : base(TAEventType.Action)
    {
    }

    public override void PerformEvent()
    {
        GV.ws.popupManager.PromptSuccess();
    }
}
