using MoreMountains.Tools;
using UnityEngine;

public struct PlayerConnectionEvent
{
    public PlayerId PlayerId;
    public Faction Faction;
    public ConnectionType ConnectionType;

    public PlayerConnectionEvent(PlayerId playerId, Faction faction, ConnectionType connectionType)
    {
        PlayerId = playerId;
        Faction = faction;
        ConnectionType = connectionType;
    }

    public static PlayerConnectionEvent e;
    public static void Trigger(PlayerId playerId, Faction faction, ConnectionType connectionType)
    {
        e.PlayerId = playerId;
        e.Faction = faction;
        e.ConnectionType = connectionType;
        MMEventManager.TriggerEvent(e);
    }
}
