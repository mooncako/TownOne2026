using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerInfo : MonoBehaviour
{
    [SerializeField, BoxGroup("Debug")] private ReadyState ReadyState = ReadyState.Preparing;
    [SerializeField, BoxGroup("Debug")] public Score Score;

    public event Action<ReadyState> OnReadyStateChanged;


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
