using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TADelegate : TAEvent {
    enum DelegateSelected { None, String, Float, Bool }
    public delegate void ParameterlessDelegate();
    public delegate void DelegateString(string s);
    public delegate void DelegateFloat(float f);
    public delegate void DelegateBool(bool b);
    ParameterlessDelegate delegateFunc;
    DelegateString delegateFuncString;
    DelegateFloat delegateFuncFloat;
    DelegateBool delegateFuncBool;
    DelegateSelected delegateSelected;
    string triggerWhenInvoke = "";
    string string1;
    float float1;
    bool bool1;

    public TADelegate(ParameterlessDelegate _delegateFunc, string _triggerWhenInvoked = "") : base(TAEventType.Action)
    {
        delegateFunc = _delegateFunc;
        triggerWhenInvoke = _triggerWhenInvoked;
        delegateSelected = DelegateSelected.None;
    }

    public TADelegate(DelegateString _delegateFunc, string arg1, string _triggerWhenInvoked = "") : base(TAEventType.Action)
    {
        delegateFuncString = _delegateFunc;
        triggerWhenInvoke = _triggerWhenInvoked;
        delegateSelected = DelegateSelected.String;
        string1 = arg1;
    }

    public TADelegate(DelegateFloat _delegateFunc, float arg2, string _triggerWhenInvoked = "") : base(TAEventType.Action)
    {
        delegateFuncFloat = _delegateFunc;
        triggerWhenInvoke = _triggerWhenInvoked;
        delegateSelected = DelegateSelected.Float;
        float1 = arg2;
    }

    public TADelegate(DelegateBool _delegateFunc, bool arg3, string _triggerWhenInvoked = "") : base(TAEventType.Action)
    {
        delegateFuncBool = _delegateFunc;
        triggerWhenInvoke = _triggerWhenInvoked;
        delegateSelected = DelegateSelected.Bool;
        bool1 = arg3;
    }

    public override void PerformEvent()
    {
        switch(delegateSelected)
        {
            case DelegateSelected.None:
                delegateFunc.Invoke();
                break;
            case DelegateSelected.String:
                delegateFuncString.Invoke(string1);
                break;
            case DelegateSelected.Float:
                delegateFuncFloat.Invoke(float1);
                break;
            case DelegateSelected.Bool:
                delegateFuncBool.Invoke(bool1);
                break;
        }
        
        if (!string.IsNullOrEmpty(triggerWhenInvoke))
            TAEventManager.Instance.ReceiveActionTrigger(triggerWhenInvoke);
    }

}
