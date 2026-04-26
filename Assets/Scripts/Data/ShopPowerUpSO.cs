using UnityEngine;

public class ShopPowerUpSO : ScriptableObject
{
    public string PowerUpId;
    public Sprite Icon;
    public float Cost;


    public virtual void ApplyPowerUp(PlayerController playerController, PlayerInfo playerInfo)
    {
        
        
    }
}
