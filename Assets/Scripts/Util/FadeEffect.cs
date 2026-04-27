using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeEffect : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    private Tween _fadeTween;

    void OnValidate()
    {
        if(_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        _canvasGroup.alpha = 1;
        _fadeTween = Tween.Alpha(_canvasGroup, 0, 1f, cycles: -1, cycleMode: CycleMode.Yoyo);
    }

    void OnDisable()
    {
        _fadeTween.Stop();
    }

}
