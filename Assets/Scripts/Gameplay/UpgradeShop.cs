using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private PlayerInfo _playerInfo;
    [SerializeField, BoxGroup("References")] private PlayerController _playerController;
    [SerializeField, BoxGroup("References")] private CanvasGroup _canvasGroup;
    [SerializeField, BoxGroup("References")] private ShopPowerUp[] _shopPowerUps;
    [SerializeField, BoxGroup("References")] private Image _shopPanel;
    [SerializeField, BoxGroup("References")] private Camera _camera;
    private int _currentSelectionIndex = 0;

    void OnValidate()
    {
        if(_playerInfo == null) _playerInfo = GetComponentInParent<PlayerInfo>();
        if(_playerController == null) _playerController = GetComponentInParent<PlayerController>();
        if(_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        _shopPowerUps = _shopPanel.GetComponentsInChildren<ShopPowerUp>();

        for(int i = 0; i < _shopPowerUps.Length; i++)
        {
            if(i == _currentSelectionIndex)
            {
                _shopPowerUps[i].OnSelect();
            }
            else
            {
                _shopPowerUps[i].OnDeselect();
            }
        }

        _playerController.OnCycleRightInput += CycleRight;
        _playerController.OnCycleLeftInput += CycleLeft;
        _playerController.OnConfirmInput += ConfirmSelection;
    }

    void OnDisable()
    {
        _playerController.OnCycleRightInput -= CycleRight;
        _playerController.OnCycleLeftInput -= CycleLeft;
        _playerController.OnConfirmInput -= ConfirmSelection;
    }

    private void ConfirmSelection(PlayerController controller, PlayerInfo info)
    {
        _shopPowerUps[_currentSelectionIndex].TryPurchase(controller, info);
    }

    private void CycleLeft()
    {
        _shopPowerUps[_currentSelectionIndex].OnDeselect();
        _currentSelectionIndex = (_currentSelectionIndex - 1 + _shopPowerUps.Length) % _shopPowerUps.Length;
        _shopPowerUps[_currentSelectionIndex].OnSelect();
    }

    private void CycleRight()
    {
        _shopPowerUps[_currentSelectionIndex].OnDeselect();
        _currentSelectionIndex = (_currentSelectionIndex + 1) % _shopPowerUps.Length;
        _shopPowerUps[_currentSelectionIndex].OnSelect();
    }
}
