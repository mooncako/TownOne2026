using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
    public int TotalRounds;
    public float RoundDuration;
    public int StartingScore;

    public GameSettings(GameSettingsSO gameSettingsSO)
    {
        TotalRounds = gameSettingsSO.TotalRounds;
        RoundDuration = gameSettingsSO.RoundDurationSeconds;
        StartingScore = gameSettingsSO.StartingScore;
    }

    public void Reset(GameSettingsSO gameSettingsSO)
    {
        TotalRounds = gameSettingsSO.TotalRounds;
        RoundDuration = gameSettingsSO.RoundDurationSeconds;
        StartingScore = gameSettingsSO.StartingScore;
    }
}
