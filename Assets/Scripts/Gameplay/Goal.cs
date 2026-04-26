using UnityEngine;

public class Goal : MonoBehaviour
{
    private PlayerInfo goalOwner;
    private PlayerInfo goalScorer => GameStateManager.Instance.GetOpponentInfo(goalOwner);
    [SerializeField] private Faction faction;
    [SerializeField] private int amt;


    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PinBall>(out var pinBall))
        {
            if (faction == Faction.Heaven)
                goalOwner = GameStateManager.Instance.HeavenPlayerInfo;
            else if (faction == Faction.Hell)
                goalOwner = GameStateManager.Instance.HellPlayerInfo;

            goalOwner.Score.UpdateScore(-amt);
            goalScorer.Score.UpdateScore(amt);

        }
    }
}