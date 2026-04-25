using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(PlayerInputManager))]
public class MenuInputManager : MonoBehaviour
{
    [SerializeField, BoxGroup("Debug"), ReadOnly] private bool _isPlayerOneTaken = false;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private bool _isPlayerTwoTaken = false;
    private bool _isShuttingDown = false;

    private void OnEnable()
    {
        _isShuttingDown = false;
    }

    private void OnDisable()
    {
        _isShuttingDown = true;
    }

    private void OnDestroy()
    {
        _isShuttingDown = true;
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        if (player.TryGetComponent(out MenuPlayer menuPlayer))
        {
            var joinDevices = GetJoinDevices(player);
            Debug.Log($"Menu join: scheme='{player.currentControlScheme}', devices=[{FormatDevices(joinDevices)}]");
            if (!_isPlayerOneTaken)
            {
                menuPlayer.Id = PlayerId.PlayerOne;
                _isPlayerOneTaken = true;
                GameStateManager.Instance.SetPlayerDeviceIds(menuPlayer.Id, joinDevices);
                Debug.Log("Menu join -> PlayerOne stored");
            }
            else if (!_isPlayerTwoTaken)
            {
                menuPlayer.Id = PlayerId.PlayerTwo;
                _isPlayerTwoTaken = true;
                GameStateManager.Instance.SetPlayerDeviceIds(menuPlayer.Id, joinDevices);
                Debug.Log("Menu join -> PlayerTwo stored");
            }
        }
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        if (_isShuttingDown)
        {
            return;
        }

        if (player.TryGetComponent(out MenuPlayer menuPlayer))
        {
            switch (menuPlayer.Id)
            {
                case PlayerId.PlayerOne:
                    _isPlayerOneTaken = false;
                    menuPlayer.Id = PlayerId.None;
                    GameStateManager.Instance.ClearPlayerDeviceIds(PlayerId.PlayerOne);
                    break;
                case PlayerId.PlayerTwo:
                    _isPlayerTwoTaken = false;
                    menuPlayer.Id = PlayerId.None;
                    GameStateManager.Instance.ClearPlayerDeviceIds(PlayerId.PlayerTwo);
                    break;
            }
        }
    }

    private static ReadOnlyArray<InputDevice> GetJoinDevices(PlayerInput player)
    {
        var scheme = player.currentControlScheme ?? string.Empty;
        if (scheme.IndexOf("keyboard", StringComparison.OrdinalIgnoreCase) >= 0
            || scheme.IndexOf("mouse", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            var devices = new List<InputDevice>(2);
            if (Keyboard.current != null)
            {
                devices.Add(Keyboard.current);
            }

            if (Mouse.current != null)
            {
                devices.Add(Mouse.current);
            }

            return new ReadOnlyArray<InputDevice>(devices.ToArray());
        }

        if (scheme.IndexOf("gamepad", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            var devices = new List<InputDevice>();
            foreach (var device in player.devices)
            {
                if (device is Gamepad)
                {
                    devices.Add(device);
                }
            }

            if (devices.Count > 0)
            {
                return new ReadOnlyArray<InputDevice>(devices.ToArray());
            }
        }

        var pairedDevices = player.user.pairedDevices;
        return pairedDevices.Count > 0 ? pairedDevices : player.devices;
    }

    private static string FormatDevices(ReadOnlyArray<InputDevice> devices)
    {
        if (devices.Count == 0)
        {
            return "none";
        }

        var parts = new string[devices.Count];
        for (var i = 0; i < devices.Count; i++)
        {
            var device = devices[i];
            parts[i] = device == null ? "null" : $"{device.displayName}:{device.deviceId}";
        }

        return string.Join(", ", parts);
    }
}