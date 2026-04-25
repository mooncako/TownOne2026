using System;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameStateManager : MMSingleton<GameStateManager>,
    MMEventListener<PlayerConnectionEvent>
{
    [SerializeField, BoxGroup("References")] private RoundManager _roundManager;
    [SerializeField, BoxGroup("References")] private GameSettingsSO _gameSettingsSO;
    public RoundManager RoundManager => _roundManager;

    [SerializeField, BoxGroup("Debug")] public GameState GameState = GameState.Preparation;
    [SerializeField, BoxGroup("Debug")] public int CurrentRound = 0;
    [SerializeField, BoxGroup("Debug")] public PlayerInfo HeavenPlayerInfo;
    [SerializeField, BoxGroup("Debug")] public PlayerId HeavenPlayerId;
    [SerializeField, BoxGroup("Debug")] public PlayerInfo HellPlayerInfo;
    [SerializeField, BoxGroup("Debug")] public PlayerId HellPlayerId;
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

        this.MMEventStartListening<PlayerConnectionEvent>();
    }

    void OnDisable()
    {
        if (_roundManager != null)
        {
            _roundManager.OnRoundStarted -= OnRoundStarted;
            _roundManager.OnRoundEnd -= OnRoundEnded;
        }

        this.MMEventStopListening<PlayerConnectionEvent>();
    }

    public void NewRound()
    {
        CurrentRound++;
        HeavenPlayerInfo.Initialize();
        HellPlayerInfo.Initialize();
        _gameSettings.Reset(_gameSettingsSO);
        _roundManager.StartRound(_gameSettings.RoundDuration);
    }

    private void OnRoundStarted()
    {
        SetState(GameState.InRound);
    }

    private void OnRoundEnded()
    {
        SetState(GameState.Result);
        var roundResult = new RoundResult(HeavenPlayerInfo.Score, HellPlayerInfo.Score);
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

    public void OnMMEvent(PlayerConnectionEvent e)
    {
        if (e.ConnectionType == ConnectionType.Connect)
        {
            if (e.Faction == Faction.Heaven)
            {
                HeavenPlayerId = e.PlayerId;
            }
            else if (e.Faction == Faction.Hell)
            {
                HellPlayerId = e.PlayerId;
            }
        }
        else if (e.ConnectionType == ConnectionType.Disconnect)
        {
            if (e.Faction == Faction.Heaven)
            {
                if(HeavenPlayerId == e.PlayerId)
                {
                    HeavenPlayerId = PlayerId.None;
                }
            }
            else if (e.Faction == Faction.Hell)
            {
                if(HellPlayerId == e.PlayerId)
                {
                    HellPlayerId = PlayerId.None;
                }
            }
        }

        if(HeavenPlayerId != PlayerId.None && HellPlayerId != PlayerId.None)
        {
            FactionChangedEvent.Trigger(true);
        }
        else
        {
            FactionChangedEvent.Trigger(false);
        }
    }
}
