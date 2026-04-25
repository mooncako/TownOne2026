using System;
using UnityEngine;

public class PlayerOwnerComp : MonoBehaviour, IHasOwner
{

    PlayerStateSO PS;
    bool bCanPossess = true;
    event Action<PlayerStateSO, PlayerStateSO> OnOwnerChanged;

    public bool CanPossess()
    {
        return bCanPossess;
    }

    public PlayerStateSO GetOwner()
    {
        return PS;
    }

    public bool SetOwner(PlayerStateSO newOwner)
    {
        if(newOwner == PS) return false;
        PlayerStateSO old_PS = PS;
        PS = newOwner;
        OnOwnerChanged?.Invoke(PS, old_PS);
        return true;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
