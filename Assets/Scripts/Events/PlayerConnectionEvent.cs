using MoreMountains.Tools;
using UnityEngine;

public struct PlayerConnectionEvent
{
    public PlayerInfo PlayerInfo;
    public Faction Faction;
    public ConnectionType ConnectionType;

    public PlayerConnectionEvent(PlayerInfo playerInfo, Faction faction, ConnectionType connectionType)
    {
        PlayerInfo = playerInfo;
        Faction = faction;
        ConnectionType = connectionType;
    }

    public static PlayerConnectionEvent e;
    public static void Trigger(PlayerInfo playerInfo, Faction faction, ConnectionType connectionType)
    {
        e.PlayerInfo = playerInfo;
        e.Faction = faction;
        e.ConnectionType = connectionType;
        MMEventManager.TriggerEvent(e);
    }
}
