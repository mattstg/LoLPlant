using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TACreatePopup : TAEvent
{
    string popupName;
    public TACreatePopup(string _popupName) : base(TAEventType.Action)
    {
        popupName = _popupName;
    }

    public override void PerformEvent()
    {
        string text = LangDict.Instance.GetText(popupName); //Text from language file using the popupname as the key
        GV.ws.popupManager.InitializePopup(text);
    }
}
