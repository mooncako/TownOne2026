using System;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    [SerializeField] private MinionSpawnPoint[] minionSpawns;

    [SerializeField] private PlayerController _playerController;


    void OnValidate()
    {
        if (minionSpawns == null || minionSpawns.Length == 0)
        {
            minionSpawns = GetComponentsInChildren<MinionSpawnPoint>();
        }
    }

    void OnEnable()
    {
        _playerController.OnArmamentApplied += PlaceMinion;
    }

    void OnDisable()
    {
        _playerController.OnArmamentApplied -= PlaceMinion;
    }

    private void PlaceMinion()
    {
        for(int i = 0; i < minionSpawns.Length; i++)
        {
            if (!minionSpawns[i].IsOccupied)
            {
                Minion mInstance = minionSpawns[i].SpawnMinion();
                mInstance.Init();
                minionSpawns[i].IsOccupied = true;
            }
        }
        
    }

}