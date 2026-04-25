using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private Button _restartButton;
    [SerializeField, BoxGroup("References")] private Button _quitButton;

    [SerializeField, BoxGroup("Settings")] private string _gameSceneName = "GameScene";

    void OnEnable()
    {
        _restartButton.onClick.AddListener(RestartGame);
        _quitButton.onClick.AddListener(QuitGame);
    }

    void OnDisable()
    {
        _restartButton.onClick.RemoveListener(RestartGame);
        _quitButton.onClick.RemoveListener(QuitGame);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }
    private void QuitGame()
    {
        Application.Quit();
    }
}
