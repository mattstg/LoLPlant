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

    public Message(string _name, Type _type = Type.Info, Position _position = Position.Bottom)
    {
        Initialize(_name, LangDict.Instance.GetText(_name), _type, _position);
    }

    public Message(string _name, string _message, Type _type = Type.Info, Position _position = Position.Bottom)
    {
        Initialize(_name, _message, _type, _position);
    }

    public void Initialize(string _name, string _message, Type _type, Position _position)
    {
        name = _name;
        message = _message;
        type = _type;
        position = _position;
        promptSuccess = false;
    }
}
