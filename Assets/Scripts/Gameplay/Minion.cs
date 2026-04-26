using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[RequireComponent(typeof(Team))]
public class Minion : MonoBehaviour, IInteract
{
    [SerializeField, BoxGroup("References")] private Team _team;

    [SerializeField, BoxGroup("Settings")] private LayerMask _pinBallLayerMask;

    [SerializeField, BoxGroup("Stats")] private float health;
    [SerializeField, BoxGroup("Stats")] private MinionData data;
    public float MaxHealth => data.Value;
    public string Name => data.Name;
    public Faction Faction => data.Faction;

    void OnValidate()
    {
        if(_team == null) _team = GetComponent<Team>();
    }

    private void Start()
    {
        health = MaxHealth;
    }

    public void Init(MinionData d)
    {
        data = d;
        switch (data.Faction)
        {
            case Faction.Heaven:
                _team.OwnerId = GameStateManager.Instance.HeavenPlayerId;
                break;
            case Faction.Hell:
                _team.OwnerId = GameStateManager.Instance.HellPlayerId;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((_pinBallLayerMask.value & (1 << collision.gameObject.layer)) != 0)
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