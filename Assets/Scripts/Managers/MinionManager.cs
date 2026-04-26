using System;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    private MinionSpawnPoint[] minionSpawns;

    [SerializeField] private List<Minion> _minionPrefabs = new List<Minion>();

    [SerializeField] private Minion _currentSelectedMinionPrefab;
    [SerializeField] private MinionSpawnPoint _currentSelectedSpawnPoint;

    [SerializeField] private PlayerController _playerController;

    private int minionSpawnPosIndex = 0;
    private int minionIndex = 0;

    private void Awake()
    {
        minionSpawns = GetComponentsInChildren<MinionSpawnPoint>();
        _currentSelectedMinionPrefab = _minionPrefabs[0];
    }

    void OnValidate()
    {
        if(_playerController == null) _playerController = GetComponentInParent<PlayerController>();
    }

    void OnEnable()
    {
        if(_playerController != null)
        {
            _playerController.OnNavigationInput += HandleNavigationInput;
            _playerController.OnConfirmInput += HandleConfirmInput;
            _playerController.OnCycleRightInput += HandleCycleRightInput;
            _playerController.OnCycleLeftInput += HandleCycleLeftInput;
            _playerController.OnReadyInput += HandleReadyInput;
        }
    }

    private void HandleReadyInput(bool isReady)
    {
        
    }

    private void HandleCycleLeftInput()
    {
        CycleMinionType(-1);
    }

    private void HandleCycleRightInput()
    {
        CycleMinionType(1);
    }

    private void HandleConfirmInput(PlayerInfo playerInfo)
    {
        PlaceMinion(playerInfo);
    }

    private void HandleNavigationInput(float input)
    {
        if(input > 0.5f)
        {
            CyclePosition(1);
        }
        else if(input < -0.5f)
        {
            CyclePosition(-1);
        }
    }
    

    private void Update()
    {

    }

    private void CyclePosition(int dir)
    {
        minionSpawnPosIndex = 
            ((minionSpawnPosIndex % minionSpawns.Length + minionSpawns.Length) + dir) % minionSpawns.Length;
        _currentSelectedSpawnPoint = minionSpawns[minionSpawnPosIndex];
        Debug.Log($"Position {minionSpawnPosIndex}");
    }

    private void CycleMinionType(int dir)
    {
        minionIndex = 
            ((minionIndex % _minionPrefabs.Count + _minionPrefabs.Count) + dir) % _minionPrefabs.Count;
        _currentSelectedMinionPrefab = _minionPrefabs[minionIndex];
        Debug.Log($"Minion type {minionIndex}");
    }

    private void PlaceMinion(PlayerInfo playerInfo)
    {
        // if CanPurchase
        if(playerInfo.Score.Value < _currentSelectedMinionPrefab.Cost)
        {
            Debug.Log("Not enough score to purchase minion");
            return;
        }
        else
        {
            playerInfo.Score.UpdateScore(-_currentSelectedMinionPrefab.Cost);
        }

        MinionSpawnPoint s = minionSpawns[minionSpawnPosIndex];

        if (!s.IsOccupied)
        {
            Minion m = SpawnMinion(_currentSelectedMinionPrefab, s.transform.position, s.transform.rotation);
            s.IsOccupied = true;
            m.SetSpawnPoint(s);

            Debug.Log($"Spawning minion {minionIndex} at position {minionSpawnPosIndex}");
        }
        else
        {
            Debug.Log("Position is occupied");
        }
        
    }
    public Minion SpawnMinion(Minion m, Vector3 position, Quaternion rotation)
    {
        Minion mInstance = Instantiate(m, position, rotation);
        mInstance.Init();
        return mInstance;
    }

}