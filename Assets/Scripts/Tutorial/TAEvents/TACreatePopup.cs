using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TACreatePopup : TAEvent
{
    List<string> popupName;

    public TACreatePopup(string _popupName) : base(TAEventType.Action)
    {
        popupName = new List<string>() { _popupName };
    }

    public TACreatePopup(List<string> _popupNames) : base(TAEventType.Action)
    {
        popupName = _popupNames;
    }

    public override void PerformEvent()
    {
        //string text = LangDict.Instance.GetText(popupName); //Text from language file using the popupname as the key
        GV.ws.popupManager.InitializePopup(popupName);
    }
}
