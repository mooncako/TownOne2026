using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputController : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private Transform _heavenSelectionPos;
    [SerializeField, BoxGroup("References")] private Transform _hellSelectionPos;
    [SerializeField, BoxGroup("References")] private MainMenuPlayerCursor _playerOneCursor;
    [SerializeField, BoxGroup("References")] private MainMenuPlayerCursor _playerTwoCursor; 
    [SerializeField, BoxGroup("Settings")] private float horizontalDeadzone = 0.5f;


    // Navigate codes are currently placeholder, need to be replaced by using playerinput manager

    private void OnPlayerOneNavigate(InputValue value)
    {
        Vector2 navigation = value.Get<Vector2>();

        if (navigation.x <= -horizontalDeadzone)
        {
            if (MoveCursorRight(_playerOneCursor, _playerTwoCursor))
            {
                PlayerConnectionEvent.Trigger(PlayerId.PlayerOne, Faction.Hell, ConnectionType.Connect);
            }else{
                PlayerConnectionEvent.Trigger(PlayerId.PlayerOne, Faction.Hell, ConnectionType.Disconnect);
                PlayerConnectionEvent.Trigger(PlayerId.PlayerOne, Faction.Heaven, ConnectionType.Disconnect);
            }
        }
        else if (navigation.x >= horizontalDeadzone)
        {
            if (MoveCursorLeft(_playerOneCursor, _playerTwoCursor))
            {
                PlayerConnectionEvent.Trigger(PlayerId.PlayerOne, Faction.Heaven, ConnectionType.Connect);
            }else{
                PlayerConnectionEvent.Trigger(PlayerId.PlayerOne, Faction.Hell, ConnectionType.Disconnect);
                PlayerConnectionEvent.Trigger(PlayerId.PlayerOne, Faction.Heaven, ConnectionType.Disconnect);
            }
        }
    }

    private void OnPlayerTwoNavigate(InputValue value)
    {
        Vector2 navigation = value.Get<Vector2>();

        if (navigation.x <= -horizontalDeadzone)
        {
            if (MoveCursorRight(_playerTwoCursor, _playerOneCursor))
            {
                PlayerConnectionEvent.Trigger(PlayerId.PlayerTwo, Faction.Hell, ConnectionType.Connect);
            }else{
                PlayerConnectionEvent.Trigger(PlayerId.PlayerTwo, Faction.Hell, ConnectionType.Disconnect);
                PlayerConnectionEvent.Trigger(PlayerId.PlayerTwo, Faction.Heaven, ConnectionType.Disconnect);
            }
        }
        else if (navigation.x >= horizontalDeadzone)
        {
            if (MoveCursorLeft(_playerTwoCursor, _playerOneCursor))
            {
                PlayerConnectionEvent.Trigger(PlayerId.PlayerTwo, Faction.Heaven, ConnectionType.Connect);
            }else{
                PlayerConnectionEvent.Trigger(PlayerId.PlayerTwo, Faction.Hell, ConnectionType.Disconnect);
                PlayerConnectionEvent.Trigger(PlayerId.PlayerTwo, Faction.Heaven, ConnectionType.Disconnect);
            }
        }
    }

    private bool MoveCursorLeft(MainMenuPlayerCursor cursor, MainMenuPlayerCursor otherCursor)
    {
        if (cursor.IsOnHeaven)
        {
            return true;
        }

        if (cursor.IsOnHell)
        {
            cursor.ResetPosition();
            return false;
        }

        if (otherCursor.IsOnHeaven)
        {
            return false;
        }

        cursor.Move(_heavenSelectionPos.localPosition, MainMenuPlayerCursor.CursorLocation.Heaven);
        return true;
    }

    private bool MoveCursorRight(MainMenuPlayerCursor cursor, MainMenuPlayerCursor otherCursor)
    {
        if (cursor.IsOnHell)
        {
            return true;
        }

        if (cursor.IsOnHeaven)
        {
            cursor.ResetPosition();
            return false;
        }

        if (otherCursor.IsOnHell)
        {
            return false;
        }

        cursor.Move(_hellSelectionPos.localPosition, MainMenuPlayerCursor.CursorLocation.Hell);
        return true;
    }
}
