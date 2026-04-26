using PrimeTween;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEventSO", menuName = "Scriptable Objects/SoundEventSO")]
public class SoundEventSO : ScriptableObject
{
    public string EventName;

    public float ReTriggerDelay = .1f;

    public bool CanTrigger = true;

    public void TriggerEvent(GameObject gameObject)
    {
        if (CanTrigger)
        {
            AkUnitySoundEngine.PostEvent(EventName, gameObject);
            CanTrigger = false;
            Tween.Delay(ReTriggerDelay).OnComplete(() => CanTrigger = true);
        }
    }
}
