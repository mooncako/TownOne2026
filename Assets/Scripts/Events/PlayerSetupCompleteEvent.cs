using MoreMountains.Tools;
using UnityEngine;

public struct PlayerSetupCompleteEvent
{
    public static PlayerSetupCompleteEvent e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);
    }
}
