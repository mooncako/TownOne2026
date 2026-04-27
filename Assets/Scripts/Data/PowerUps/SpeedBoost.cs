using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoost", menuName = "PowerUps/SpeedBoost")]
public class SpeedBoost : ShopPowerUpSO
{
    [SerializeField, BoxGroup("Settings")] private float _speedMultiplier = 1.5f;
    [SerializeField, BoxGroup("Settings")] private float _duration = 5f; 

    public override void ApplyPowerUp(PlayerController playerController, PlayerInfo playerInfo)
    {
        base.ApplyPowerUp(playerController, playerInfo);

        playerController.ApplySpeedBoost(_speedMultiplier, _duration);

        
    }
}
