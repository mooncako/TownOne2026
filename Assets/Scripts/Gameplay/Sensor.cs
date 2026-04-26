using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            OnSensorTriggered();
        }
    }

    protected virtual void OnSensorTriggered(){ }
}