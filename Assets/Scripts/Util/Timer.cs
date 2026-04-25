using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float Duration { get; private set; }
    public float ElapsedTime { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action OnTimerComplete;

    public void StartTimer(float duration)
    {
        Duration = duration;
        ElapsedTime = 0f;
        IsRunning = true;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    void Update()
    {
        if (IsRunning)
        {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime >= Duration)
            {
                ElapsedTime = Duration;
                IsRunning = false;
                OnTimerComplete?.Invoke();
            }
        }
    }
}
