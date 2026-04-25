using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinionManager : MonoBehaviour
{
    [SerializeField] private MinionData[] minionTypes;
    [SerializeField] private MinionSpawnPoint[] minionSpawns;

    [SerializeField] private Minion minionPrefab;

    int minionSpawnIndex = 0;
    int minionTypeIndex = 0;

    private void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame) CyclePosition(1);
        if (Keyboard.current.downArrowKey.wasPressedThisFrame) CyclePosition(-1);

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame) CycleMinionType(1);
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame) CycleMinionType(-1);

        if (Keyboard.current.spaceKey.wasPressedThisFrame) PlaceMinion();
    }

    private void CyclePosition(int dir)
    {
        minionSpawnIndex = (minionSpawnIndex + dir) % minionSpawns.Length;
        Debug.Log($"Position {minionSpawnIndex}");
    }

    private void CycleMinionType(int dir)
    {
        minionTypeIndex = (minionTypeIndex + dir) % minionTypes.Length;
        Debug.Log($"Minion type {minionTypeIndex}");
    }

    private void PlaceMinion()
    {
        // if CanPurchase

        MinionSpawnPoint s = minionSpawns[minionSpawnIndex];

        if (!s.IsOccupied)
        {
            Minion m = SpawnMinion(minionTypes[minionTypeIndex], s.transform.position);
            s.IsOccupied = true;

            Debug.Log($"Spawning minion {minionTypeIndex} at position {minionSpawnIndex}");
        }
        else
        {
            Debug.Log("Position is occupied");
        }
        
    }
    public Minion SpawnMinion(MinionData d, Vector3 position)
    {
        Minion m = Instantiate(minionPrefab, position, Quaternion.identity);
        m.Init(d);
        return m;
    }

}