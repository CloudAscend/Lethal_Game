using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyCodeEvent 
{
    public KeyCodeEvent()
    {

    }

    public void AddKeyEvent(Func<bool> key,System.Action value)
    {
        _onKeyEvent.Add(key,value);
    }

    public void RemoveKeyEvent(Func<bool> key)
    {
        _onKeyEvent.Remove(key);
    }

    public System.Action GetKeyEvent(Func<bool> key)
    {
        return _onKeyEvent[key];
    }

    public Dictionary<Func<bool>, System.Action> GetKeyEvents()
    {
        return _onKeyEvent;
    }
    
    private readonly Dictionary<Func<bool>, System.Action> _onKeyEvent = new();
}
