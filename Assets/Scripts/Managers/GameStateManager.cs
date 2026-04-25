using System;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameStateManager : MMSingleton<GameStateManager>
{
    [SerializeField, BoxGroup("References")] private RoundManager _roundManager;
    public RoundManager RoundManager => _roundManager;

    [SerializeField, BoxGroup("Debug")] public GameState GameState;
    [SerializeField, BoxGroup("Debug")] public int CurrentRound = 0;


    void OnValidate()
    {
        if (_roundManager == null) _roundManager = GetComponent<RoundManager>();
    }

    void OnEnable()
    {
        if (_roundManager != null)
        {
            _roundManager.OnRoundStarted += OnRoundStarted;
            _roundManager.OnRoundEnd += OnRoundEnded;
        }
    }

    void OnDisable()
    {
        if (_roundManager != null)
        {
            _roundManager.OnRoundStarted -= OnRoundStarted;
            _roundManager.OnRoundEnd -= OnRoundEnded;
        }
    }

    public void NewRound()
    {
        CurrentRound++;
        _roundManager.StartRound();
    }

    private void OnRoundStarted()
    {
        SetState(GameState.InRound);
    }

    private void OnRoundEnded()
    {
        SetState(GameState.Result);
    }


    private void SetState(GameState state)
    {
        GameState = state;
    }


}
