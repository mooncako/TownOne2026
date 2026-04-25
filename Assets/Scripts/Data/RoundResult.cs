using System;
using UnityEngine;

[Serializable]
public class RoundResult
{
    public Score HeavenPlayerScore;
    public Score HellPlayerScore;

    public RoundResult(Score heavenPlayerState, Score hellPlayerState)
    {
        HeavenPlayerScore.Copy(heavenPlayerState);
        HellPlayerScore.Copy(hellPlayerState);
    }
}
