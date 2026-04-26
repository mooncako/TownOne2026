using MoreMountains.Tools;
using UnityEngine;

public struct SensorHitEvent
{
    public PlayerId Team;
    public Powerup Powerup;

    public SensorHitEvent(PlayerId team, Powerup powerup)
    {
        Team = team;
        Powerup = powerup;
    }

    public static SensorHitEvent e;
    public static void Trigger(PlayerId team, Powerup powerup)
    {
        e.Team = team;
        e.Powerup = powerup;
        MMEventManager.TriggerEvent(e);
    }
}
