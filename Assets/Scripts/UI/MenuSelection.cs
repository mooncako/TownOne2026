using UnityEngine;
using UnityEngine.Events;

public class MenuSelection : MonoBehaviour
{
    [SerializeField] private FadeEffect _fadeEffect;
    public bool IsSelected = false;

    public UnityEvent OnTrigger;

    void OnValidate()
    {
        if(_fadeEffect == null) _fadeEffect = GetComponentInChildren<FadeEffect>();
    }

    void OnEnable()
    {
        if(IsSelected)
        {
            _fadeEffect.gameObject.SetActive(true);
        }else
        {
            _fadeEffect.gameObject.SetActive(false);
        }
    }

    public void OnSelect()
    {
        IsSelected = true;
        _fadeEffect.gameObject.SetActive(true);
    }

    public void OnDeselect()
    {
        IsSelected = false;
        _fadeEffect.gameObject.SetActive(false);
    }

    public void Trigger()
    {
        if(IsSelected)
        {
            OnTrigger?.Invoke();
        }
    }
}
