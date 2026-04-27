using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CanvasGroup))]
public class ButtonPanel : MonoBehaviour,
    MMEventListener<FactionChangedEvent>,
    MMEventListener<MenuPlayerInputEvent>,
    MMEventListener<MenuPlayerSubmitEvent>
{
    [SerializeField, BoxGroup("References")] private CanvasGroup _canvasGroup;
    [SerializeField, BoxGroup("References")] private List<MenuSelection> _menuSelections;

    [SerializeField, BoxGroup("Settings")] private string _gameSceneName = "GameScene";

    private int _currentSelectionIndex = 0;

    void Awake()
    {
        EnsureReferences();
        ApplyVisibility(false);
    }

    void OnValidate()
    {
        if(_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    void OnEnable()
    {
        EnsureReferences();
        this.MMEventStartListening<FactionChangedEvent>();
        this.MMEventStartListening<MenuPlayerInputEvent>();
        this.MMEventStartListening<MenuPlayerSubmitEvent>();
    }

    void OnDisable()
    {
        this.MMEventStopListening<FactionChangedEvent>();
        this.MMEventStopListening<MenuPlayerInputEvent>();
        this.MMEventStopListening<MenuPlayerSubmitEvent>();
    }

    public void StartGame()
    {
        if(!GameStateManager.Instance.IsBothPlayersConnected())
        {
            return;
        }

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.BeginSceneTransition();
        }

        SceneManager.LoadScene(_gameSceneName);
    }

    public void OnMMEvent(FactionChangedEvent e)
    {
        if(GameStateManager.Instance.IsBothPlayersConnected())
        {
            ApplyVisibility(true);
        }
    }

    private void EnsureReferences()
    {
        if(_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void ApplyVisibility(bool isVisible)
    {
        if(isVisible)
        {
            Tween.Alpha(_canvasGroup, 1, 0.5f);
        }else
        {
            Tween.Alpha(_canvasGroup, 0, 0.5f);
        }
        _canvasGroup.interactable = isVisible;
        _canvasGroup.blocksRaycasts = isVisible;
    }

    private bool AreBothPlayersConnected()
    {
        if(GameStateManager.Instance == null) return false;
        return GameStateManager.Instance.HeavenPlayerId != PlayerId.None
            && GameStateManager.Instance.HellPlayerId != PlayerId.None;
    }

    public void OnMMEvent(MenuPlayerInputEvent e)
    {
        if(e.Input.y > 0.5f)
        {
            CycleUp();
        }
        else if(e.Input.y < -0.5f)
        {
            CycleDown();
        }
    }

    private void CycleUp()
    {
        _currentSelectionIndex = (_currentSelectionIndex - 1 + _menuSelections.Count) % _menuSelections.Count;
        UpdateMenuSelection();
    }

    private void CycleDown()
    {
        _currentSelectionIndex = (_currentSelectionIndex + 1) % _menuSelections.Count;
        UpdateMenuSelection();
    }

    private void UpdateMenuSelection()
    {

        for (int i = 0; i < _menuSelections.Count; i++)
        {
            _menuSelections[i].OnDeselect();
        }

        _menuSelections[_currentSelectionIndex].OnSelect();

    }

    public void OnMMEvent(MenuPlayerSubmitEvent e)
    {
        _menuSelections[_currentSelectionIndex].Trigger();
    }
}
