using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using fjj.core;

public class EventCenter:Singleton<EventCenter>
{

    public Dictionary<string, Action<object>> EventActions= new Dictionary<string, Action<object>>();
    
    public void On(string key, Action<object> evt)
    {
        if (!EventActions.ContainsKey(key))
        {
            EventActions.Add(key, evt);
        }
        else
        {
            EventActions[key] += evt;
        }
    }
    

    public void Broad(string key, object paras)
    {
        if (EventActions.ContainsKey(key))
        {
            EventActions[key].Invoke(paras);
        }
    }
}
