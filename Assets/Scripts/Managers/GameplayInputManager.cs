using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputManager : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private PlayerController _hellPlayer;
    [SerializeField, BoxGroup("References")] private PlayerController _heavenPlayer;
    [SerializeField, BoxGroup("References")] private PlayerInputManager _playerInputManager;
    [SerializeField, BoxGroup("Settings")] private bool _disablePlayerInputManager = true;

    void Start()
    {
        AssignControllers();
    }

    private void AssignControllers()
    {
        var gameState = GameStateManager.Instance;
        if (gameState == null)
        {
            return;
        }

        gameState.NormalizeDeviceAssignments();
        Debug.Log($"Gameplay assign: P1=[{FormatIds(gameState.PlayerOneDeviceIds)}], P2=[{FormatIds(gameState.PlayerTwoDeviceIds)}]");

        if (_disablePlayerInputManager)
        {
            if (_playerInputManager == null)
            {
                _playerInputManager = PlayerInputManager.instance;
            }

            if (_playerInputManager != null)
            {
                _playerInputManager.enabled = false;
            }
        }

        if (_heavenPlayer != null)
        {
            _heavenPlayer.AssignDevicesFromGameState(gameState.HeavenPlayerId);
        }

        if (_hellPlayer != null)
        {
            _hellPlayer.AssignDevicesFromGameState(gameState.HellPlayerId);
        }
    }

    private static string FormatIds(List<int> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return "none";
        }

        return string.Join(", ", ids);
    }
}
