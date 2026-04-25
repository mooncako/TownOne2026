using MoreMountains.Tools;
using UnityEngine;

public struct RoundStateChangeEvent
{
    public RoundState RoundState;
    public RoundStateChangeEvent(RoundState roundState)
    {
        RoundState = roundState;
    }

    public static RoundStateChangeEvent e;
    public static void Trigger(RoundState roundState)
    {
        e.RoundState = roundState;
        MMEventManager.TriggerEvent(e);
    }
}
