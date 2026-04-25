using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CanvasGroup))]
public class StartButton : MonoBehaviour,
    MMEventListener<FactionChangedEvent>
{
    [SerializeField, BoxGroup("References")] private Button _button;
    [SerializeField, BoxGroup("References")] private CanvasGroup _canvasGroup;

    [SerializeField, BoxGroup("Settings")] private string _gameSceneName = "GameScene";

    void Awake()
    {
        EnsureReferences();
        ApplyVisibility(false);
    }

    void OnValidate()
    {
        if(_button == null) _button = GetComponent<Button>();
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
        _button.onClick.AddListener(StartGame);
        this.MMEventStartListening<FactionChangedEvent>();
        ApplyVisibility(AreBothPlayersConnected());
    }

    void OnDisable()
    {
        _button.onClick.RemoveListener(StartGame);
        this.MMEventStopListening<FactionChangedEvent>();
    }

    private void StartGame()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.BeginSceneTransition();
        }

        SceneManager.LoadScene(_gameSceneName);
    }

    public void OnMMEvent(FactionChangedEvent e)
    {
        ApplyVisibility(e.IsBothPlayersConnected);
    }

    private void EnsureReferences()
    {
        if(_button == null) _button = GetComponent<Button>();
        if(_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void ApplyVisibility(bool isVisible)
    {
        _canvasGroup.alpha = isVisible ? 1f : 0f;
        _canvasGroup.interactable = isVisible;
        _canvasGroup.blocksRaycasts = isVisible;
    }

    private bool AreBothPlayersConnected()
    {
        if(GameStateManager.Instance == null) return false;
        return GameStateManager.Instance.HeavenPlayerId != PlayerId.None
            && GameStateManager.Instance.HellPlayerId != PlayerId.None;
    }
}
