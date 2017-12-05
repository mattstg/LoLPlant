using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAPromptSuccess : TAEvent
{
    private string messageName;


    public TAPromptSuccess(string _messageName) : base(TAEventType.Action)
    {
        messageName = _messageName;
    }

    public override void PerformEvent()
    {
        GV.ws.popupManager.PromptSuccess(messageName);
    }
}
