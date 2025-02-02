using System;
using System.Collections.Generic;
using UnityEngine;


public class PubSub : MonoBehaviour
{
    private static Dictionary<string, Action<object>> _events = new Dictionary<string, Action<object>>();

    public static void Subscribe<T1, T2>(string eventName, Action<T1, T2> listener)
    {
        if (!_events.ContainsKey(eventName))
        {
            _events[eventName] = obj => { };
        }
        _events[eventName] += obj =>
        {
            var data = ((T1, T2))obj;
            listener(data.Item1, data.Item2);
        };
    }

    public static void Unsubscribe<T1, T2>(string eventName, Action<T1, T2> listener)
    {
        if (_events.ContainsKey(eventName))
        {
            _events[eventName] -= obj =>
            {
                var data = ((T1, T2))obj; // Cast alla tupla
                listener(data.Item1, data.Item2);
            };

            if (_events[eventName] == null || _events[eventName].GetInvocationList().Length == 0)
            {
                _events.Remove(eventName);
            }
        }
    }

    public static void Publish<T1, T2>(string eventName, T1 param1, T2 param2)
    {
        if (_events.ContainsKey(eventName))
        {
            _events[eventName].Invoke((param1, param2));
        }
    }
}
