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
    [SerializeField, BoxGroup("FillDisplay")] private Image _heavenDisplay;
    [SerializeField, BoxGroup("FillDisplay")] private Image _hellDisplay;

    [SerializeField, BoxGroup("FillDisplay")] private TMP_Text _heavenLabel;
    [SerializeField, BoxGroup("FillDisplay")] private TMP_Text _hellLabel;

    private float heavenTotal;
    private float hellTotal;

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
        GameStateManager.Instance.RoundManager
    }

    public void OnMMEvent(ScoreChangeEvent e)
    {
        
        if (e.Owner == GameStateManager.Instance.HeavenPlayerInfo)
        {
            e.NewScore = heavenTotal;
        }
        else
        {
            e.NewScore = hellTotal;
        }

        UpdateScoreDisplay();
        
    }

    private void UpdateScoreDisplay()
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

        _heavenLabel.text = heavenTotal.ToString();
        _hellLabel.text = hellTotal.ToString();
    }

}

