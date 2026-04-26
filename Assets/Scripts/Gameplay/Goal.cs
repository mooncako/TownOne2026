using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private PlayerId goalOwnerID;
    [SerializeField] private int amt;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PinBall>(out var pinBall))
        {
            PlayerInfo goalOwner = GameStateManager.Instance.GetPlayerInfoFromID(goalOwnerID);
            PlayerInfo goalScorer = GameStateManager.Instance.GetOpponentInfo(goalOwner);

            goalOwner.Score.UpdateScore(-amt);
            goalScorer.Score.UpdateScore(amt);

        }
    }
}