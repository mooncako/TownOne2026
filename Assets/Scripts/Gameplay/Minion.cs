using Sirenix.OdinInspector;
using UnityEngine;

public class Minion : MonoBehaviour
{
    [SerializeField, BoxGroup("Stats")] private float health;
    [SerializeField, BoxGroup("Stats")] private MinionData data;
    public float MaxHealth => data.Value;
    public string Name => data.Name;

    private void Start()
    {
        health = MaxHealth;
    }

    public void Init(MinionData d)
    {
        data = d;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            health -= 1;
            if (health <= 0) DestroyMinion();
        }
    }

    private void DestroyMinion()
    {
        Debug.Log("Destroyed");
        data.MinionSpawnPoint.IsOccupied = false;
        gameObject.SetActive(false);
    }
}