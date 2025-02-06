using UnityEngine;

public class EventData
{
}
public class BubbleThrownEvent : EventData
{
    public GameObject Player;
}

public class BubbleDestroyedEvent : EventData
{ }

public class BubbleGrabbedEvent : EventData
{
    public GameObject Parent;
}

public class PlayerLevelUpEvent : EventData {}
