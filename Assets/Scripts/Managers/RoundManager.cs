using System;
using MoreMountains.Tools;
using UnityEngine;

public class RoundManager : MonoBehaviour
{

    public event Action OnRoundStarted;
    public event Action OnRoundEnd;

    public void StartRound()
    {
        // Timer starts here
        OnRoundStarted.Invoke();
    }


    private void EndRound()
    {
        // register to the timer end event
        OnRoundEnd.Invoke();
    }
}
