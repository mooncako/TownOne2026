using UnityEngine;

[CreateAssetMenu(fileName = "MinionData", menuName = "Scriptable Objects/MinionData")]
public class MinionData : ScriptableObject
{
    [SerializeField]
    private int mValue;

    [SerializeField]
    private string mName;

    private MinionSpawnPoint mSpawnPoint;

    public int Value => mValue;
    public string Name => mName;
    public MinionSpawnPoint MinionSpawnPoint => mSpawnPoint;
}
