using System;
using UnityEngine;

[Serializable]
public class RoundResult
{
    public Score HeavenPlayerScore = new Score();
    public Score HellPlayerScore = new Score();

    public RoundResult(Score heavenPlayerState, Score hellPlayerState)
    {
        HeavenPlayerScore.Copy(heavenPlayerState);
        HellPlayerScore.Copy(hellPlayerState);
    }
}
