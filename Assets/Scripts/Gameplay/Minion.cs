using UnityEngine;

public class Minion : MonoBehaviour
{
    private float health;
    [SerializeField] private float maxHealth = 1;
    private void Start()
    {
        health = maxHealth;
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