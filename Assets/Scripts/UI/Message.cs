using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public enum Type { Info, Prompt }
    public enum Position { Center, Top, Bottom, Left, Right }

    public string name;
    public string message;
    public Type type;
    public Position position;
    public bool promptSuccess;

    public Message(string messageName, Type messageType = Type.Info, Position messagePosition = Position.Bottom)
    {
        name = messageName;
        message = LangDict.Instance.GetText(name);
        type = messageType;
        position = messagePosition;
        promptSuccess = false;
    }
}
