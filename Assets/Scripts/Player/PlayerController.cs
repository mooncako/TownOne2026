using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerInfo))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private PlayerInfo _playerInfo;
    [SerializeField, BoxGroup("References")] private Rigidbody _rigidBody;

    [SerializeField, BoxGroup("Settings | Movement")] private float _movementSpeed = 5f;
    [SerializeField, BoxGroup("Settings | Movement")] private float _rangeX = 1f;
    [SerializeField, BoxGroup("Settings | Flip")] private float _flipAngle = 15f;
    [SerializeField, BoxGroup("Settings | Flip")] private float _flipRotationSpeed = 360f;

    [SerializeField, BoxGroup("Debug"), ReadOnly] private float _movementInput;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private float _rotationInput;


    private float _targetAngle = 0f;

    private float GetCurrentAngle()
    {
        float angle = _rigidBody.rotation.eulerAngles.y;
        if(angle > 180f) angle -= 360f;
        return angle;
    }

    void OnValidate()
    {
        if(_playerInfo == null) _playerInfo = GetComponent<PlayerInfo>();
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
        if(_rigidBody == null) _rigidBody = GetComponentInChildren<Rigidbody>();
        ApplyRigidbodySettings();
    }

    private void ApplyRigidbodySettings()
    {
        if(_rigidBody == null) return;
        _rigidBody.isKinematic = false;
        _rigidBody.useGravity = false;
        _rigidBody.constraints = RigidbodyConstraints.FreezePositionY
            | RigidbodyConstraints.FreezePositionZ
            | RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        // Movement
        float targetX = _rigidBody.position.x + _movementInput * _movementSpeed * Time.fixedDeltaTime;
        _rigidBody.MovePosition(new Vector3(targetX, _rigidBody.position.y, _rigidBody.position.z));

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
        _targetAngle = _flipAngle * Mathf.Clamp(direction, -1f, 1f);
    }

}
