using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MenuInputManager : MonoBehaviour
{
    [SerializeField, BoxGroup("Debug"), ReadOnly] private bool _isPlayerOneTaken = false;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private bool _isPlayerTwoTaken = false;

    private void OnPlayerJoined(PlayerInput player)
    {
        if (player.TryGetComponent(out MenuPlayer menuPlayer))
        {
            if (!_isPlayerOneTaken)
            {
                menuPlayer.Id = PlayerId.PlayerOne;
                _isPlayerOneTaken = true;
            }
            else if (!_isPlayerTwoTaken)
            {
                menuPlayer.Id = PlayerId.PlayerTwo;
                _isPlayerTwoTaken = true;
            }
        }
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        if (player.TryGetComponent(out MenuPlayer menuPlayer))
        {
            switch (menuPlayer.Id)
            {
                case PlayerId.PlayerOne:
                    _isPlayerOneTaken = false;
                    menuPlayer.Id = PlayerId.None;
                    break;
                case PlayerId.PlayerTwo:
                    _isPlayerTwoTaken = false;
                    menuPlayer.Id = PlayerId.None;
                    break;
            }
        }
    }
}