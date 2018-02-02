using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public enum Type { Info, Prompt, Endgame }
    public enum Position { Center, Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight }

    public string name;
    public string message;
    public string ttsName;
    public Type type;
    public Position position;
    public bool promptSuccess;

    public Message(string _name, string _ttsName, Type _type = Type.Info, Position _position = Position.Center)
    {
        Initialize(_name, LangDict.Instance.GetText(_name), _ttsName, _type, _position);
    }

    public Message(string _name, string _message, string _ttsName, Type _type = Type.Info, Position _position = Position.Center)
    {
        Initialize(_name, _message, _ttsName, _type, _position);
    }

    public void Initialize(string _name, string _message, string _ttsName, Type _type, Position _position)
    {
        name = _name;
        message = _message;
        ttsName = _ttsName;
        type = _type;
        position = _position;
        promptSuccess = false;
    }
}
