using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
    public int TotalRounds;
    public float RoundDurationSeconds;
    public int StartingScore;

    public GameSettings(GameSettingsSO gameSettingsSO)
    {
        TotalRounds = gameSettingsSO.TotalRounds;
        RoundDurationSeconds = gameSettingsSO.RoundDurationSeconds;
        StartingScore = gameSettingsSO.StartingScore;
    }

    public void Reset(GameSettingsSO gameSettingsSO)
    {
        TotalRounds = gameSettingsSO.TotalRounds;
        RoundDurationSeconds = gameSettingsSO.RoundDurationSeconds;
        StartingScore = gameSettingsSO.StartingScore;
    }
}
