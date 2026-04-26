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

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if(GameStateManager.Instance != null)
        {
            float timeInSeconds = GameStateManager.Instance.RoundManager.RoundTimer.Countdown;
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
            _timer.text = timerText;
        }

    }

    public void OnMMEvent(ScoreChangeEvent e)
    {
        
        if (GameStateManager.Instance.GetPlayerIDFromInfo(e.Owner) == PlayerId.PlayerOne)
        {
            e.NewScore = p1total;
        }
        else
        {
            e.NewScore = p2total;
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
    }

}

