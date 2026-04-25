using System;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameStateManager : MMSingleton<GameStateManager>
{
    [SerializeField, BoxGroup("References")] private RoundManager _roundManager;
    [SerializeField, BoxGroup("References")] private GameSettingsSO _gameSettingsSO;
    public RoundManager RoundManager => _roundManager;

    [SerializeField, BoxGroup("Debug")] public GameState GameState = GameState.Preparation;
    [SerializeField, BoxGroup("Debug")] public int CurrentRound = 0;
    [SerializeField, BoxGroup("Debug")] public PlayerState HeavenPlayerState;
    [SerializeField, BoxGroup("Debug")] public PlayerState HellPlayerState;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private GameSettings _gameSettings;


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

        DontDestroyOnLoad(gameObject);

        if(Instance != this)
        {
            Destroy(gameObject);
        }

        if(_gameSettingsSO != null)
        {
            _gameSettings = new GameSettings(_gameSettingsSO);
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
        HeavenPlayerState.Initialize();
        HellPlayerState.Initialize();
        _gameSettings.Reset(_gameSettingsSO);
    }

    private void OnRoundStarted()
    {
        SetState(GameState.InRound);
    }

    private void OnRoundEnded()
    {
        SetState(GameState.Result);
        var roundResult = new RoundResult(HeavenPlayerState.Score, HellPlayerState.Score);
        _roundManager.AssignRoundResult(roundResult, CurrentRound - 1);

        if(CurrentRound >= _roundManager.RoundResults.Length)
        {
            // Game End
            SetState(GameState.GameEnd);
        }
    }


    private void SetState(GameState state)
    {
        GameState = state;
    }


}
