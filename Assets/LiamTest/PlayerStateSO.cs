using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "PlayerStateSO", menuName = "Scriptable Objects/PlayerStateSO")]
public class PlayerStateSO : ScriptableObject
{
    int Score = 0;
    event Action<int> OnScoreChanged;
    int GetScore()
    {
        return Score;
    }

    bool SetScore(int newScore)
    {
        Score = newScore;
        OnScoreChanged?.Invoke(Score);
        return true;
    }

    bool ModifySocre(int value)
    {
        Score += value;
        OnScoreChanged?.Invoke(Score);
        return true;
    }
}
