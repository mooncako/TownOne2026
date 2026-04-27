using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class GameStateManager : MMSingleton<GameStateManager>,
    MMEventListener<PlayerConnectionEvent>,
    MMEventListener<PlayerSetupCompleteEvent>
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
    [ShowInInspector, BoxGroup("Debug")] public List<int> PlayerOneDeviceIds = new List<int>();
    [ShowInInspector, BoxGroup("Debug")] public List<int> PlayerTwoDeviceIds = new List<int>();
    [SerializeField, BoxGroup("Debug"), ReadOnly] private GameSettings _gameSettings;
    private string _gameOverSceneName = "EndScreen";

    [ShowInInspector, BoxGroup("Debug"), ReadOnly] public bool IsSceneTransitioning { get; private set; } = false;


    protected override void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
            _roundManager.OnPreparationEnded += OnPreparationEnded;
        }

        if(_gameSettingsSO != null)
        {
            _gameSettings = new GameSettings(_gameSettingsSO);
        }

        this.MMEventStartListening<PlayerConnectionEvent>();
        this.MMEventStartListening<PlayerSetupCompleteEvent>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    

    void OnDisable()
    {
        if (_roundManager != null)
        {
            _roundManager.OnRoundStarted -= OnRoundStarted;
            _roundManager.OnRoundEnd -= OnRoundEnded;
            _roundManager.OnPreparationEnded -= OnPreparationEnded;
        }

        this.MMEventStopListening<PlayerConnectionEvent>();
        this.MMEventStopListening<PlayerSetupCompleteEvent>();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.buildIndex == 0)
        {
            SetState(GameState.Preparation);
            CurrentRound = 0;
            HeavenPlayerInfo = null;
            HellPlayerInfo = null;
            HeavenPlayerId = PlayerId.None;
            HellPlayerId = PlayerId.None;
        }
    }

    public void NewGame()
    {
        // CurrentRound++;
        HeavenPlayerInfo.Initialize(_gameSettings.StartingScore);

        HellPlayerInfo.Initialize(_gameSettings.StartingScore);
        _gameSettings.Reset(_gameSettingsSO);
        _roundManager.StartPreparation(3f);

        // _roundManager.StartRound(_gameSettings.RoundDuration);
    }

    private void OnPreparationEnded()
    {
        PreparationEndedEvent.Trigger();
    }

    public void NewRound()
    {
        CurrentRound++;
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
            SceneManager.LoadScene(_gameOverSceneName);
        }
    }


    private void SetState(GameState state)
    {
        GameState = state;
        GameStateChangeEvent.Trigger(state);
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

    public bool IsBothPlayersConnected()
    {
        return HeavenPlayerId != PlayerId.None && HellPlayerId != PlayerId.None;
    }

    public RoundManager GetRoundManager()
    {
        return _roundManager;
    }

    public void BeginSceneTransition()
    {
        IsSceneTransitioning = true;
    }

    public void EndSceneTransition()
    {
        IsSceneTransitioning = false;
    }

    public void SetPlayerDeviceIds(PlayerId playerId, IReadOnlyList<InputDevice> devices)
    {
        var targetList = GetDeviceIdList(playerId);
        if (targetList == null)
        {
            return;
        }

        var otherList = playerId == PlayerId.PlayerOne ? PlayerTwoDeviceIds : PlayerOneDeviceIds;

        targetList.Clear();
        for (var i = 0; i < devices.Count; i++)
        {
            var deviceId = devices[i].deviceId;
            if (!targetList.Contains(deviceId))
            {
                targetList.Add(deviceId);
            }

            if (otherList != null)
            {
                otherList.Remove(deviceId);
            }
        }

        Debug.Log($"SetPlayerDeviceIds: {playerId} -> [{string.Join(", ", targetList)}]");
    }

    public void ClearPlayerDeviceIds(PlayerId playerId)
    {
        var targetList = GetDeviceIdList(playerId);
        if (targetList == null)
        {
            return;
        }

        targetList.Clear();
        Debug.Log($"ClearPlayerDeviceIds: {playerId}");
    }

    public void NormalizeDeviceAssignments()
    {
        if (PlayerOneDeviceIds.Count == 0 && PlayerTwoDeviceIds.Count > 1)
        {
            TrySplitDeviceLists(PlayerTwoDeviceIds, PlayerOneDeviceIds);
        }
        else if (PlayerTwoDeviceIds.Count == 0 && PlayerOneDeviceIds.Count > 1)
        {
            TrySplitDeviceLists(PlayerOneDeviceIds, PlayerTwoDeviceIds);
        }
    }

    private static void TrySplitDeviceLists(List<int> source, List<int> target)
    {
        if (target.Count > 0 || source.Count <= 1)
        {
            return;
        }

        var hasKeyboard = false;
        var hasMouse = false;
        var gamepadId = -1;

        for (var i = 0; i < source.Count; i++)
        {
            var device = InputSystem.GetDeviceById(source[i]);
            if (device is Keyboard)
            {
                hasKeyboard = true;
            }
            else if (device is Mouse)
            {
                hasMouse = true;
            }
            else if (device is Gamepad && gamepadId == -1)
            {
                gamepadId = source[i];
            }
        }

        if (hasKeyboard && hasMouse && gamepadId == -1)
        {
            return;
        }

        if (gamepadId != -1)
        {
            source.Remove(gamepadId);
            target.Add(gamepadId);
            return;
        }

        var moveId = source[source.Count - 1];
        source.RemoveAt(source.Count - 1);
        target.Add(moveId);
    }

    private List<int> GetDeviceIdList(PlayerId playerId)
    {
        switch (playerId)
        {
            case PlayerId.PlayerOne:
                return PlayerOneDeviceIds;
            case PlayerId.PlayerTwo:
                return PlayerTwoDeviceIds;
            default:
                return null;
        }
    }

    public PlayerInfo GetPlayerInfoFromID(PlayerId playerId)
    {
        return playerId switch
        {
            _ when playerId == HeavenPlayerId => HeavenPlayerInfo,
            _ when playerId == HellPlayerId => HellPlayerInfo,
            _ => throw new ArgumentException($"Unknown PlayerId: {playerId}", nameof(playerId))
        };
    }

    public PlayerInfo GetOpponentInfo(PlayerInfo playerInfo) =>
    playerInfo == HeavenPlayerInfo ? HellPlayerInfo : HeavenPlayerInfo;

    public PlayerId GetPlayerIDFromInfo(PlayerInfo playerInfo)
    {
        return playerInfo switch
        {
            _ when playerInfo == HeavenPlayerInfo => HeavenPlayerId,
            _ when playerInfo == HellPlayerInfo => HellPlayerId,
            _ => throw new ArgumentException($"Unknown PlayerInfo: {playerInfo}", nameof(playerInfo))
        };
    }

    public PlayerId GetOpponentID(PlayerId playerId) =>
    playerId == HeavenPlayerId ? HellPlayerId : HeavenPlayerId;

    public void OnMMEvent(PlayerSetupCompleteEvent e)
    {
        HeavenPlayerInfo = e.HeavenPlayerInfo;
        HellPlayerInfo = e.HellPlayerInfo;
        HeavenPlayerInfo.OnReadyStateChanged += OnReadyStateChanged;
        HellPlayerInfo.OnReadyStateChanged += OnReadyStateChanged;
        SetState(GameState.Preparation);
        NewGame();
    }

    private void OnReadyStateChanged(ReadyState state)
    {
        if (state == ReadyState.Ready)
        {
            if (HeavenPlayerInfo.ReadyState == ReadyState.Ready && HellPlayerInfo.ReadyState == ReadyState.Ready)
            {
                NewRound();
            }
        }
    }

    public PlayerInfo GetPlayerInfo(PlayerId playerId)
    {
        if(playerId == HeavenPlayerId)
        {
            return HeavenPlayerInfo;
        }
        else if(playerId == HellPlayerId)
        {
            return HellPlayerInfo;
        }
        else
        {
            return null;
        }
    }

    public PlayerInfo GetOpposedPlayerInfo(PlayerId playerId)
    {
        if(playerId == HeavenPlayerId)
        {
            return HellPlayerInfo;
        }
        else if(playerId == HellPlayerId)
        {
            return HeavenPlayerInfo;
        }
        else
        {
            return null;
        }
    }

}
