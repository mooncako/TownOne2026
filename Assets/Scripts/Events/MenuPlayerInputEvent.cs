using MoreMountains.Tools;
using UnityEngine;

public struct MenuPlayerInputEvent
{
    public Vector2 Input;
    public PlayerId Id;
    public MenuPlayerInputEvent(Vector2 input, PlayerId id)
    {
        Input = input;
        Id = id;
    }

    public static MenuPlayerInputEvent e;
    public static void Trigger(Vector2 input, PlayerId id)
    {
        e.Input = input;
        e.Id = id;
        MMEventManager.TriggerEvent(e);
    }
}
