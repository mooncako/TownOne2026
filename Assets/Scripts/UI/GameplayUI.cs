using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour,
    MMEventListener<ScoreChangeEvent>
{
    [SerializeField, BoxGroup("FillDisplay")] private Image _p1Display;
    [SerializeField, BoxGroup("FillDisplay")] private Image _p2Display;

    [SerializeField, BoxGroup("Score")] private TMP_Text _p1Score;
    [SerializeField, BoxGroup("Score")] private TMP_Text _p2Score;

    [SerializeField, BoxGroup("Timer")] private TMP_Text _timer;

    private float p1total;
    private float p2total;

    private void OnEnable()
    {
        this.MMEventStartListening<ScoreChangeEvent>();
    }
    private void OnDisable()
    {
        this.MMEventStopListening<ScoreChangeEvent>();
    }

    void Start()
    {
        // HARDCODED CAUSE THE GAME SETTINGS IS PRIVATE...
        _p1Score.text = "1000";
        _p2Score.text = "1000";
        UpdateScoreDisplay();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if(GameStateManager.Instance != null)
        {
            float timeInSeconds = GameStateManager.Instance.RoundManager.RoundTimer.Countdown;
            string timerText;

            if (GameStateManager.Instance.GameState == GameState.Preparation)
            {
                timerText = Mathf.FloorToInt(timeInSeconds+1).ToString();
            }
            else if (GameStateManager.Instance.GameState == GameState.InRound)
            {
                int minutes = Mathf.FloorToInt(timeInSeconds / 60);
                int seconds = Mathf.FloorToInt(timeInSeconds % 60);
                timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                timerText = "";
            }

            
            _timer.text = timerText;
        }

    }

    public void OnMMEvent(ScoreChangeEvent e)
    {
        
        if (GameStateManager.Instance.GetPlayerIDFromInfo(e.Owner) == PlayerId.PlayerOne)
        {
            p1total = e.Owner.Score.Value;
        }
        else
        {
            p2total = e.Owner.Score.Value;
        }

        UpdateScoreDisplay();
        
    }

    private void UpdateScoreDisplay()
    {
        float combinedTotal = p1total + p2total;

        if (combinedTotal == 0f)
        {
            _p1Display.fillAmount = 0.5f;
            _p2Display.fillAmount = 0.5f;
            return;
        }

        // Fill
        _p1Display.fillAmount = p1total / combinedTotal;
        _p2Display.fillAmount = p2total / combinedTotal;

        //Text
        _p1Score.text = p1total.ToString();
        _p2Score.text = p2total.ToString();
    }

}

