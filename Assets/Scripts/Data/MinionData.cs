using UnityEngine;

[CreateAssetMenu(fileName = "MinionData", menuName = "Scriptable Objects/MinionData")]
public class MinionData : ScriptableObject
{
    [SerializeField]
    private int value;

    [SerializeField]
    private string minionName;

    public int Value => value;
    public string MinionName => minionName;
}
