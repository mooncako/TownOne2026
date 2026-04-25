using System;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [field:SerializeField, BoxGroup("Result")] public RoundResult[] RoundResults = new RoundResult[3];
    public event Action OnRoundStarted;
    public event Action OnRoundEnd;

    public void StartRound()
    {
        // Timer starts here
        OnRoundStarted.Invoke();
        RoundStateChangeEvent.Trigger(RoundState.Started);
    }


    private void EndRound()
    {
        // register to the timer end event
        OnRoundEnd.Invoke();
        RoundStateChangeEvent.Trigger(RoundState.Ended);
    }

    public void AssignRoundResult(RoundResult result, int roundIndex)
    {
        if (roundIndex < 0 || roundIndex >= RoundResults.Length)
        {
            Debug.LogError("Invalid round index");
            return;
        }

        RoundResults[roundIndex] = result;
    }
}
