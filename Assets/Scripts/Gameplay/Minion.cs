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
    [SerializeField] private MinionSpawnPoint mSpawnPoint;
    public float MaxHealth => data.Value;
    public string Name => data.Name;
    public Faction Faction => data.Faction;
    public float Cost => data.Cost;
    public MinionSpawnPoint MinionSpawnPoint => mSpawnPoint;

    void OnValidate()
    {
        if(_team == null) _team = GetComponent<Team>();
    }

    private void Start()
    {
        health = MaxHealth;
    }

    public void Init()
    {
        switch (data.Faction)
        {
            case Faction.Heaven:
                _team.OwnerId = GameStateManager.Instance.HeavenPlayerId;
                break;
            case Faction.Hell:
                _team.OwnerId = GameStateManager.Instance.HellPlayerId;
                break;
        }

        SetSoundSwitch();
        AkUnitySoundEngine.PostEvent("OBJ_Minion_Set", gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((_pinBallLayerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            
        }

        
    }

    private void DestroyMinion()
    {
        Debug.Log("Destroyed");
        mSpawnPoint.IsOccupied = false;
        SetSoundSwitch();
        AkUnitySoundEngine.PostEvent("OBJ_Minion_Die", gameObject);
        Destroy(gameObject);
    }

    public bool Interact(GameObject Instigator, string Action = "")
    {
        if(Instigator.TryGetComponent(out Team instigatorTeam) && health == 1)
        {
            if(instigatorTeam.OwnerId != _team.OwnerId)
            {
                PlayerInfo info = GameStateManager.Instance.GetPlayerInfo(instigatorTeam.OwnerId);
                if(info != null)
                {
                    info.Score.UpdateScore(Cost/2);
                }
            }
        }

        if(Instigator.TryGetComponent<IPhysics>(out IPhysics comp))
        {
            Vector3 normal = Instigator.GetComponent<Collider>().ClosestPoint(transform.position) - transform.position;
            normal = Vector3.ProjectOnPlane(normal, Vector3.up);
            comp.AddImpulse(normal * 20f);
            PostSound();
            
        }
        health -= 1;

        if(health <= 0)
        {
            DestroyMinion();
        }

        return true;
    }

    public List<string> GetInteractOptions(GameObject Instigator = null)
    {
        return new List<string>() { "" };
    }

    public bool Interact(GameObject Instigator, Action callback = null)
    {
        return true;
    }

    public void SetSpawnPoint(MinionSpawnPoint spawnPoint)
    {
        mSpawnPoint = spawnPoint;
    }

    private void PostSound()
    {   
        SetSoundSwitch();

        AkUnitySoundEngine.PostEvent("COLL_Minion", gameObject);
    }

    private void SetSoundSwitch()
    {
        switch(data.Faction)
        {
            case Faction.Heaven:
                AkUnitySoundEngine.SetSwitch("MinionType", "Heaven", gameObject);
                break;
            case Faction.Hell:
                AkUnitySoundEngine.SetSwitch("MinionType", "Hell", gameObject);
                break;
        }

        switch(data.Size)
        {
            case Size.Small:
                AkUnitySoundEngine.SetSwitch("MinionSize", "Small", gameObject);
                break;
            case Size.Med:
                AkUnitySoundEngine.SetSwitch("MinionSize", "Med", gameObject);
                break;
            case Size.Heavy:
                AkUnitySoundEngine.SetSwitch("MinionSize", "Heavy", gameObject);
                break;
        }
    }
}