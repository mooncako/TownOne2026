using TMPro;
using UnityEngine;

public class ScoreBoardEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text round;
    [SerializeField] private TMP_Text heavenScore;
    [SerializeField] private TMP_Text hellScore;

    public void SetValues(int roundIndex, float heaven, float hell)
    {
        roundIndex++;
        
        round.text = roundIndex.ToString();
        heavenScore.text = heaven.ToString();
        hellScore.text = hell.ToString();
    }
}
