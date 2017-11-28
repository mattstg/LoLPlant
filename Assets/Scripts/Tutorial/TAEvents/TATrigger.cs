using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TATrigger : TAEvent
{
    public string triggerKey;

    public TATrigger(string _triggerKey) : base(TAEventType.Trigger)
    {
        triggerKey = _triggerKey;
    }

    public override void PerformEvent()
    {
        TAEventManager.Instance.RecieveLock(triggerKey);
    }
}
