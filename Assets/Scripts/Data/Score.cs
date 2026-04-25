using System;
using UnityEngine;

[Serializable]
public class Score
{
    public float Value;

    public void Copy(Score other)
    {
        Value = other.Value;
    }
}
