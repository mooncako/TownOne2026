using MoreMountains.Tools;
using UnityEngine;

public struct ScoreChangeEvent
{
    public float NewScore;
    public PlayerInfo Owner;

    public ScoreChangeEvent(float newScore, PlayerInfo owner)
    {
        NewScore = newScore;
        Owner = owner;
    }

    public static ScoreChangeEvent e;
    public static void Trigger(float newScore, PlayerInfo owner)
    {
        e.NewScore = newScore;
        e.Owner = owner;
        MMEventManager.TriggerEvent(e);
    }
}
