using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TADelegate : TAEvent {

    public delegate void ParameterlessDelegate();
    ParameterlessDelegate delegateFunc;
    string triggerWhenInvoke = "";

    public TADelegate(ParameterlessDelegate _delegateFunc, string _triggerWhenInvoked = "") : base(TAEventType.Action)
    {
        delegateFunc = _delegateFunc;
        triggerWhenInvoke = _triggerWhenInvoked;
    }

    public override void PerformEvent()
    {
        delegateFunc.Invoke();
        if (!string.IsNullOrEmpty(triggerWhenInvoke))
            TAEventManager.Instance.ReceiveActionTrigger(triggerWhenInvoke);
    }

}
