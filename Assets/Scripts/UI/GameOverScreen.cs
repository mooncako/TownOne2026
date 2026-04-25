using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private Button _restartButton;
    [SerializeField, BoxGroup("References")] private Button _quitButton;

    [SerializeField, BoxGroup("Settings")] private string _gameSceneName = "GameScene";

    [SerializeField, BoxGroup("FillDisplay")] private Image _heavenDisplay;
    [SerializeField, BoxGroup("FillDisplay")] private Image _hellDisplay;

    [SerializeField, BoxGroup("FillDisplay")] private TMP_Text _heavenLabel;
    [SerializeField, BoxGroup("FillDisplay")] private TMP_Text _hellLabel;

    [SerializeField, BoxGroup("FillDisplay")] private TMP_Text _heavenWin;
    [SerializeField, BoxGroup("FillDisplay")] private TMP_Text _hellWin;

    [SerializeField, BoxGroup("Scoreboard")] private ScoreBoardEntry _scoreBoardEntryPrefab;
    [SerializeField, BoxGroup("Scoreboard")] private RectTransform _scoreBoardGroup;


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

    private void Start()
    {

        StartCoroutine(CalculateScoreTestRoutine());
    }

    private IEnumerator CalculateScoreRoutine()
    {
        float heavenTotalScore = 0;
        float hellTotalScore = 0;
        RoundResult[] results = GameStateManager.Instance.GetRoundManager().RoundResults;

        for (int i = 0; i < results.Length; i++)
        {
            yield return new WaitForSeconds(1f);

            float heavenScore = results[i].HeavenPlayerScore.Value;
            float hellScore = results[i].HellPlayerScore.Value;
            heavenTotalScore += heavenScore;
            hellTotalScore += hellScore;
            AddScoreboardEntry(i, heavenScore, hellScore);
            UpdateRatioFill(heavenTotalScore, hellTotalScore);
        }

        _heavenWin.gameObject.SetActive(heavenTotalScore > hellTotalScore);
        _hellWin.gameObject.SetActive(heavenTotalScore <= hellTotalScore);
    }

    private IEnumerator CalculateScoreTestRoutine()
    {
        float heavenTotalScore = 0;
        float hellTotalScore = 0;
        float[] heavenScores = { 100, 200, 400 };
        float[] hellScores = { 300, 200, 50 };

        for (int i = 0; i < heavenScores.Length; i++)
        {
            yield return new WaitForSeconds(1f);

            float heavenScore = heavenScores[i];
            float hellScore = hellScores[i];
            heavenTotalScore += heavenScore;
            hellTotalScore += hellScore;
            AddScoreboardEntry(i, heavenScore, hellScore);
            UpdateRatioFill(heavenTotalScore, hellTotalScore);
        }

        _heavenWin.gameObject.SetActive(heavenTotalScore > hellTotalScore);
        _hellWin.gameObject.SetActive(heavenTotalScore <= hellTotalScore);

    }
    private void AddScoreboardEntry(int roundIndex, float heavenScore, float hellScore)
    {
        ScoreBoardEntry e = Instantiate(_scoreBoardEntryPrefab, _scoreBoardGroup);
        e.SetValues(roundIndex, heavenScore, hellScore);
    }

    private void UpdateRatioFill(float heavenTotal, float hellTotal)
    {
        float combinedTotal = heavenTotal + hellTotal;

        if (combinedTotal == 0f)
        {
            _heavenDisplay.fillAmount = 0.5f;
            _hellDisplay.fillAmount = 0.5f;
            _heavenLabel.text = "0";
            _hellLabel.text = "0";
            return;
        }

        // Fill
        _heavenDisplay.fillAmount = heavenTotal / combinedTotal;
        _hellDisplay.fillAmount = hellTotal / combinedTotal;

        // Label
        float fullHeight = 150f;

        float heavenCenterY = Mathf.Lerp(fullHeight, -fullHeight, _heavenDisplay.fillAmount / 2);
        float hellCenterY = Mathf.Lerp(-fullHeight, fullHeight, _hellDisplay.fillAmount / 2);

        _heavenLabel.rectTransform.anchoredPosition = new Vector2(
            _heavenLabel.rectTransform.anchoredPosition.x, heavenCenterY);

        _hellLabel.rectTransform.anchoredPosition = new Vector2(
            _hellLabel.rectTransform.anchoredPosition.x, hellCenterY);

        _heavenLabel.text = heavenTotal.ToString();
        _hellLabel.text = hellTotal.ToString();
    }

}

