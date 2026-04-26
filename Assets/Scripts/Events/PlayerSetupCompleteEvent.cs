using MoreMountains.Tools;
using UnityEngine;

public struct PlayerSetupCompleteEvent
{
    public PlayerInfo HeavenPlayerInfo;
    public PlayerInfo HellPlayerInfo;

    public PlayerSetupCompleteEvent(PlayerInfo heavenPlayerInfo, PlayerInfo hellPlayerInfo)
    {
        HeavenPlayerInfo = heavenPlayerInfo;
        HellPlayerInfo = hellPlayerInfo;
    }

    public static PlayerSetupCompleteEvent e;
    public static void Trigger(PlayerInfo heavenPlayerInfo, PlayerInfo hellPlayerInfo)
    {
        e.HeavenPlayerInfo = heavenPlayerInfo;
        e.HellPlayerInfo = hellPlayerInfo; 
        MMEventManager.TriggerEvent(e);
    }
}
