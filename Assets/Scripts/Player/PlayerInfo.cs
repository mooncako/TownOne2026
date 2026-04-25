using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerInfo : MonoBehaviour
{
    [SerializeField, BoxGroup("Debug")] public ReadyState ReadyState = ReadyState.Preparing;
    [SerializeField, BoxGroup("Debug")] public Score Score;

    public event Action<ReadyState> OnReadyStateChanged;

    public void InitializeScore(float value)
    {
        Score.Value = value;
    }

    public void Initialize()
    {
        ToggleReadyState(ReadyState.Preparing);
    }

    public void ToggleReadyState(ReadyState state)
    {
        ReadyState = state;
        OnReadyStateChanged?.Invoke(ReadyState);
    }



}
