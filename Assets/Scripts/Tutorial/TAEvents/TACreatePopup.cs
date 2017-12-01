using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TACreatePopup : TAEvent
{
    List<Message> messages;

    public TACreatePopup(Message _message) : base(TAEventType.Action)
    {
        messages = new List<Message>() { _message };
    }

    public TACreatePopup(List<Message> _messages) : base(TAEventType.Action)
    {
        messages = _messages;
    }

    public override void PerformEvent()
    {
        GV.ws.popupManager.LoadPopup(messages);
    }
}
