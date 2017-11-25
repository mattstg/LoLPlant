using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TACreatePopup : TAEvent
{
    string popupName;
    public TACreatePopup(TAEventType _eventType, string _eventName, string _popupName) : base(_eventType, _eventName)
    {
        popupName = _popupName;
    }

    public override void PerformEvent()
    {
        string text = LangDict.Instance.GetText(popupName); //Text from language file using the popupname as the key
        GV.ws.popupManager.InitializePopup(text);
    }
}
