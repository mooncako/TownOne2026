using MoreMountains.Tools;
using UnityEngine;

public struct FactionChangedEvent
{
    public bool IsBothPlayersConnected;
    public FactionChangedEvent(bool isBothPlayersConnected)
    {
        IsBothPlayersConnected = isBothPlayersConnected;
    }

    public static FactionChangedEvent e;
    public static void Trigger(bool isBothPlayersConnected)
    {
        e.IsBothPlayersConnected = isBothPlayersConnected;
        MMEventManager.TriggerEvent(e);
    }
}
