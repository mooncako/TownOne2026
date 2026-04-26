using MoreMountains.Tools;
using UnityEngine;

public struct GameStateChangeEvent
{
    public GameState CurrentState;
    public GameStateChangeEvent(GameState currentState)
    {
        CurrentState = currentState;
    }

    public static GameStateChangeEvent e;
    public static void Trigger(GameState currentState)
    {
        e.CurrentState = currentState;
        MMEventManager.TriggerEvent(e);
     }
}
