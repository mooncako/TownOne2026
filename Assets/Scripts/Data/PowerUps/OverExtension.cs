using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "OverExtension", menuName = "PowerUps/OverExtension")]
public class OverExtension : ShopPowerUpSO
{
    [SerializeField, BoxGroup("Settings")] private float _extensionMultiplier = 1.5f;
    [SerializeField, BoxGroup("Settings")] private float _duration = 5f;

    public override void ApplyPowerUp(PlayerController playerController, PlayerInfo playerInfo)
    {
        base.ApplyPowerUp(playerController, playerInfo);

        playerController.ExtendOverExtension(_extensionMultiplier, _duration);
    }
}
