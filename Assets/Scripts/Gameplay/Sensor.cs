using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] private Powerup powerup;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PinBall>(out var pinBall))
        {
            SensorHitEvent.Trigger(pinBall.Team.OwnerId, powerup);
        }
    }
}