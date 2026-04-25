using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerState
{
    [SerializeField, BoxGroup("Debug")] public ReadyState ReadyState = ReadyState.Preparing;
    [SerializeField, BoxGroup("Debug")] public Score Score;


    public void Initialize()
    {
        ReadyState = ReadyState.Preparing;
    }

    public void WriteScore()
    {
        
    }

    public void Copy(PlayerState other)
    {
        ReadyState = other.ReadyState;
        Score.Copy(other.Score);
    }
}
