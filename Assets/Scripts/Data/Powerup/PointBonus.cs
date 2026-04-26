using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Powerups/PointBonus")]
public class PointBonus : Powerup
{
    [SerializeField] private int amt;

    public override void ApplyEffect(PlayerInfo playerInfo)
    {
        playerInfo.Score.UpdateScore(amt);
    }
}
