using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopPowerUp : MonoBehaviour
{
    [SerializeField] private Image _selection;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private ShopPowerUpSO _shopPowerUp;

    void OnEnable()
    {
        if(_shopPowerUp != null)
        {
            _icon.sprite = _shopPowerUp.Icon;
            _costText.text = _shopPowerUp.Cost.ToString();
        }
    }

    public bool TryPurchase(PlayerController playerController, PlayerInfo playerInfo)
    {
        if(_shopPowerUp == null) return false;
        if (playerInfo.Score.Value >= _shopPowerUp.Cost)
        {
            playerInfo.Score.UpdateScore(-_shopPowerUp.Cost);
            _shopPowerUp.ApplyPowerUp(playerController, playerInfo);
            return true;
        }

        return false;
    }

    public void OnSelect()
    {
        if (_selection != null)
        {
            _selection.enabled = true;
        }
    }

    public void OnDeselect()
    {
        if (_selection != null)
        {
            _selection.enabled = false;
        }
    }
}
