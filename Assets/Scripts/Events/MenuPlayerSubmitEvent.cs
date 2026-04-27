using MoreMountains.Tools;
using UnityEngine;

public struct MenuPlayerSubmitEvent
{
    public static MenuPlayerSubmitEvent e;
    public static void Trigger()
    {
        MMEventManager.TriggerEvent(e);
    }
}
