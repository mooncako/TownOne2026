using System;
using UnityEngine;

[Serializable]
public class Score
{
    public float Value;
    public event Action<float> OnScoreChanged;

    public Score(float initialValue = 0)
    {
        Value = initialValue;
    }

    public void Copy(Score other)
    {
        Value = other.Value;
    }

    public void UpdateScore(float amount)
    {
        Value += amount;
        OnScoreChanged?.Invoke(Value);
    }
}
