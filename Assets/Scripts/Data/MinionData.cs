using UnityEngine;

public enum Size
{
    Small,
    Med,
    Heavy
}


[CreateAssetMenu(fileName = "MinionData", menuName = "Scriptable Objects/MinionData")]
public class MinionData : ScriptableObject
{
    [SerializeField]
    private int mValue;

    [SerializeField]
    private string mName;

    [SerializeField]
    private Faction mFaction;

    [SerializeField]
    private float mCost;

    [SerializeField]
    private Size mSize;

    

    public int Value => mValue;
    public string Name => mName;
    public Faction Faction => mFaction;
    public float Cost => mCost;
    public Size Size => mSize;
}
