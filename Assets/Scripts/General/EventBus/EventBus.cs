using System;
using System.Collections.Generic;

public abstract class EventBus
{
    private static readonly Dictionary<Type, Delegate> Events = new();

    public static void Raise<T>(EventData data) where T : EventData
    {
        Type type = typeof(T);
        
        if (Events.TryGetValue(type, out Delegate existingAction))
        {
            existingAction?.DynamicInvoke();
        }
    }

    public static void Subscribe<T>(Action<T> action) where T : EventData
    {
        Type type = typeof(T);

        if (Events.ContainsKey(type))
        {
            Events[type] = Delegate.Combine(Events[type], action);
        }
        else
        {
            Events[type] = action;
        }
    }

    public static void Unsubscribe<T>(Action<T> action) where T : EventData
    {
        Type type = typeof(T);

        if (Events.ContainsKey(type))
        {
            Events[type] = Delegate.Remove(Events[type], action);
        }
    }
}
