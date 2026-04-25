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
            var joinDevice = GetJoinDevice(player);
            if (!_isPlayerOneTaken)
            {
                menuPlayer.Id = PlayerId.PlayerOne;
                _isPlayerOneTaken = true;
                GameStateManager.Instance.PlayerOneDevice = joinDevice;
            }
            else if (!_isPlayerTwoTaken)
            {
                menuPlayer.Id = PlayerId.PlayerTwo;
                _isPlayerTwoTaken = true;
                GameStateManager.Instance.PlayerTwoDevice = joinDevice;
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
                    GameStateManager.Instance.PlayerOneDevice = null;
                    break;
                case PlayerId.PlayerTwo:
                    _isPlayerTwoTaken = false;
                    menuPlayer.Id = PlayerId.None;
                    GameStateManager.Instance.PlayerTwoDevice = null;
                    break;
            }
        }
    }

    private static InputDevice GetJoinDevice(PlayerInput player)
    {
        var pairedDevices = player.user.pairedDevices;
        if (pairedDevices.Count > 0)
        {
            return pairedDevices[0];
        }

        return player.devices.Count > 0 ? player.devices[0] : null;
    }
}