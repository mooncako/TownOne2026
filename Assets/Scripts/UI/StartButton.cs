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
        _button.onClick.AddListener(StartGame);
        this.MMEventStartListening<FactionChangedEvent>();
    }

    void OnDisable()
    {
        _button.onClick.RemoveListener(StartGame);
        this.MMEventStopListening<FactionChangedEvent>();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void OnMMEvent(FactionChangedEvent e)
    {
        if(e.IsBothPlayersConnected)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        else
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
