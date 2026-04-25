using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] public float Duration;
    [SerializeField] public float ElapsedTime;
    [SerializeField] public bool IsRunning;

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
