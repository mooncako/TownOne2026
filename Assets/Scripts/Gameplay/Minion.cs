using UnityEngine;

public class Minion : MonoBehaviour
{
    private float health;
    private MinionData data;
    public float MaxHealth => data.Value;
    public string Name => data.MinionName;
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
        gameObject.SetActive(false);
    }
}