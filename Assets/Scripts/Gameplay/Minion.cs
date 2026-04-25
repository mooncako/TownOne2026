using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Minion : MonoBehaviour, IInteract
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

    public bool Interact(GameObject Instigator, string Action = "")
    {
        
        return true;
    }

    public List<string> GetInteractOptions(GameObject Instigator = null)
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(GameObject Instigator, Action callback = null)
    {
        throw new NotImplementedException();
    }
}