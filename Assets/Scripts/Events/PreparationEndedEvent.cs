using MoreMountains.Tools;
using UnityEngine;

public struct PreparationEndedEvent
{
    public static PreparationEndedEvent e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);
    }
}
