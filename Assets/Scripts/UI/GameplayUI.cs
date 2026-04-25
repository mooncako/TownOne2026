using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField, BoxGroup("FillDisplay")] private Image _heavenDisplay;
    [SerializeField, BoxGroup("FillDisplay")] private Image _hellDisplay;

    [SerializeField, BoxGroup("FillDisplay")] private TMP_Text _heavenLabel;
    [SerializeField, BoxGroup("FillDisplay")] private TMP_Text _hellLabel;

    private float heavenTotal;
    private float hellTotal;

    private void Start()
    {
        heavenTotal = 0;
        hellTotal = 0;
        UpdateRatioFill();
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            heavenTotal += 1;
            UpdateRatioFill();
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            hellTotal += 1;
            UpdateRatioFill();
        }

    }

    private void UpdateRatioFill()
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

