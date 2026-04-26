using UnityEngine;

[CreateAssetMenu(fileName = "Armament", menuName = "PowerUps/Armament")]
public class Armament : ShopPowerUpSO
{
    public override void ApplyPowerUp(PlayerController playerController, PlayerInfo playerInfo)
    {
        base.ApplyPowerUp(playerController, playerInfo);
        playerController.ApplyArmament();
    }
}
