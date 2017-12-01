using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public enum Type { Info, Prompt }
    public enum Position { Center, Top, Bottom, Left, Right }

    public string message;
    public Type type;
    public Position position;

    public Message(string messageName, Type messageType = Type.Info, Position messagePosition = Position.Bottom)
    {
        message = LangDict.Instance.GetText(messageName);
        type = messageType;
        position = messagePosition;
    }
}
