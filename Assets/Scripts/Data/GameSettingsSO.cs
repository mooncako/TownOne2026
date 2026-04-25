using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "TO2/GameSettingsSO")]
public class GameSettingsSO : ScriptableObject
{
    public int TotalRounds = 3;
    public float RoundDurationSeconds = 120;
    public int StartingScore = 1000;

}
