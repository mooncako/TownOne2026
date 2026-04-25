using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerInfo))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private PlayerInfo _playerInfo;
    [SerializeField, BoxGroup("References")] private Rigidbody _rigidBody;
    [SerializeField, BoxGroup("References")] private PlayerInput _playerInput;

    [SerializeField, BoxGroup("Settings | Movement")] private float _movementSpeed = 5f;
    [SerializeField, BoxGroup("Settings | Movement")] private float _rangeX = 1f;
    [SerializeField, BoxGroup("Settings | Flip")] private float _flipAngle = 15f;
    [SerializeField, BoxGroup("Settings | Flip")] private float _flipRotationSpeed = 360f;

    [SerializeField, BoxGroup("Debug"), ReadOnly] private float _movementInput;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private float _rotationInput;


    private float _targetAngle = 0f;
    private float _baseAngle = 0f;

    private float GetCurrentAngle()
    {
        float angle = _rigidBody.rotation.eulerAngles.y;
        if(angle > 180f) angle -= 360f;
        return angle;
    }

    void OnValidate()
    {
        if(_playerInfo == null) _playerInfo = GetComponent<PlayerInfo>();
        if(_playerInput == null) _playerInput = GetComponent<PlayerInput>();
        if(_rigidBody == null) 
        {
            _rigidBody = GetComponentInChildren<Rigidbody>();
            _rigidBody.useGravity = false;
        }
        ApplyRigidbodySettings();
    }

    void Awake()
    {
        if(_playerInfo == null) _playerInfo = GetComponent<PlayerInfo>();
        if(_playerInput == null) _playerInput = GetComponent<PlayerInput>();
        if(_rigidBody == null) _rigidBody = GetComponentInChildren<Rigidbody>();
        ApplyRigidbodySettings();
        _baseAngle = GetCurrentAngle();
        _targetAngle = _baseAngle;
    }

    private void ApplyRigidbodySettings()
    {
        if(_rigidBody == null) return;
        _rigidBody.isKinematic = true;
        _rigidBody.useGravity = false;
        _rigidBody.constraints = RigidbodyConstraints.FreezePositionY
            | RigidbodyConstraints.FreezePositionZ
            | RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        // Movement
        var right = _rigidBody.rotation * Vector3.right;
        right.y = 0f;
        right.Normalize();
        var moveDelta = right * (_movementInput * _movementSpeed * Time.fixedDeltaTime);
        _rigidBody.MovePosition(_rigidBody.position + moveDelta);

        float currentAngle = GetCurrentAngle();
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, _targetAngle, _flipRotationSpeed * Time.fixedDeltaTime);
        _rigidBody.MoveRotation(Quaternion.Euler(0f, newAngle, 0f));
    }

    private void OnMovement(InputValue value)
    {
        _movementInput = value.Get<float>();
    }

    private void OnRotate(InputValue value)
    {
        _rotationInput = value.Get<float>();
        SetFlip(_rotationInput);
    }

    private void OnConfirm(InputValue value)
    {
        if(value.isPressed)
        {
            
        }
    }

    private void OnReady(InputValue value)
    {
        if(value.isPressed)
        {
            switch(_playerInfo.ReadyState)
            {
                case ReadyState.Preparing:
                    _playerInfo.ToggleReadyState(ReadyState.Ready);
                    break;
                case ReadyState.Ready:
                    _playerInfo.ToggleReadyState(ReadyState.Preparing);
                    break;
            }
        }
    }

    private void SetFlip(float direction)
    {
        _targetAngle = _baseAngle + _flipAngle * Mathf.Clamp(direction, -1f, 1f);
    }

    public bool AssignDevicesFromGameState(PlayerId playerId)
    {
        if (_playerInput == null)
        {
            _playerInput = GetComponent<PlayerInput>();
            if (_playerInput == null)
            {
                return false;
            }
        }

        var gameState = GameStateManager.Instance;
        if (gameState == null)
        {
            return false;
        }

        List<int> deviceIds = null;
        switch (playerId)
        {
            case PlayerId.PlayerOne:
                deviceIds = gameState.PlayerOneDeviceIds;
                break;
            case PlayerId.PlayerTwo:
                deviceIds = gameState.PlayerTwoDeviceIds;
                break;
            default:
                return false;
        }

        if (deviceIds == null || deviceIds.Count == 0)
        {
            if (_playerInput.actions != null)
            {
                _playerInput.actions.devices = new ReadOnlyArray<InputDevice>(Array.Empty<InputDevice>());
            }

            _playerInput.DeactivateInput();
            return false;
        }

        var resolvedDevices = new List<InputDevice>(deviceIds.Count);
        for (var i = 0; i < deviceIds.Count; i++)
        {
            var device = InputSystem.GetDeviceById(deviceIds[i]);
            if (device != null)
            {
                resolvedDevices.Add(device);
            }
        }

        if (resolvedDevices.Count == 0)
        {
            return false;
        }

        if (!_playerInput.user.valid)
        {
            _playerInput.ActivateInput();
            if (!_playerInput.user.valid)
            {
                return false;
            }
        }

        UnpairDevicesFromOtherUsers(_playerInput.user, resolvedDevices);

        _playerInput.neverAutoSwitchControlSchemes = true;
        _playerInput.user.UnpairDevices();

        for (var i = 0; i < resolvedDevices.Count; i++)
        {
            InputUser.PerformPairingWithDevice(resolvedDevices[i], _playerInput.user);
        }

        if (_playerInput.actions != null)
        {
            _playerInput.actions.devices = new ReadOnlyArray<InputDevice>(resolvedDevices.ToArray());
        }

        if (!string.IsNullOrWhiteSpace(_playerInput.currentControlScheme))
        {
            _playerInput.SwitchCurrentControlScheme(_playerInput.currentControlScheme, resolvedDevices.ToArray());
        }

        return _playerInput.user.pairedDevices.Count > 0;
    }

    private static void UnpairDevicesFromOtherUsers(InputUser targetUser, List<InputDevice> devices)
    {
        if (devices.Count == 0)
        {
            return;
        }

        var users = InputUser.all;
        for (var i = 0; i < users.Count; i++)
        {
            var user = users[i];
            if (!user.valid || user == targetUser)
            {
                continue;
            }

            for (var d = 0; d < devices.Count; d++)
            {
                var device = devices[d];
                if (device != null && user.pairedDevices.ContainsReference(device))
                {
                    user.UnpairDevice(device);
                }
            }
        }
    }

}
