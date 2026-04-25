using UnityEngine;

public class MinionManager : MonoBehaviour
{
    [SerializeField] private MinionData[] minionTypes;
    [SerializeField] private Vector3[] availablePositions;

    [SerializeField] private Minion minionPrefab;
    public Minion SpawnMinion(MinionData d, Vector3 position)
    {
        Minion m = Instantiate(minionPrefab, position, Quaternion.identity);
        m.Init(d);
        return m;
    }

    private void Start()
    {
        
    }
}