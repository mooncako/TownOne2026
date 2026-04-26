using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputManager : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private GameObject _playerPrefab;
    [SerializeField, BoxGroup("References")] private Transform _heavenSpawnPoint;
    [SerializeField, BoxGroup("References")] private Transform _hellSpawnPoint;
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

        gameState.EndSceneTransition();

        gameState.NormalizeDeviceAssignments();

        PlayerController heavenController = Instantiate(_playerPrefab, _heavenSpawnPoint.position, _heavenSpawnPoint.rotation).GetComponent<PlayerController>();
        heavenController.AssignDevicesFromGameState(GameStateManager.Instance.HeavenPlayerId);
        heavenController.SetTeam(GameStateManager.Instance.HeavenPlayerId);
        PlayerController hellController = Instantiate(_playerPrefab, _hellSpawnPoint.position, _hellSpawnPoint.rotation).GetComponent<PlayerController>();
        hellController.AssignDevicesFromGameState(GameStateManager.Instance.HellPlayerId);
        hellController.SetTeam(GameStateManager.Instance.HellPlayerId);
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
