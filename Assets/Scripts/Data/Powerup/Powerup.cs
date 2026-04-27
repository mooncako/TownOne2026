using UnityEngine;

public abstract class Powerup : ScriptableObject
{
    public void ApplyEffect(PlayerId team)
    {
        ApplyEffect(GameStateManager.Instance.GetPlayerInfoFromID(team));
    }

    public abstract void ApplyEffect(PlayerInfo playerInfo);

}
