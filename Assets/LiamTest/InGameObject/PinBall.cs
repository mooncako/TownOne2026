using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PinBall : MonoBehaviour, IPhysics, IInteract
{
    [SerializeField, BoxGroup("References")] private Rigidbody _rigidBody;
    [SerializeField, BoxGroup("References")] private Team _team;
    [SerializeField, BoxGroup("Settings")] private LayerMask _interactableLayerMask;
    [SerializeField, BoxGroup("Settings")] private LayerMask _paddleLayerMask;

    public float SpeedMultiplierOverTime = 0.0f;
    protected Vector2 SpeedMultiplierClamp = new Vector2(0.0f, 15.0f);
    protected Vector2 SpeedMultiplierRemap = new Vector2(1.0f, 5.0f);


    private void OnValidate()
    {
        if(_rigidBody == null) _rigidBody = GetComponent<Rigidbody>();
        if(_team == null) _team = GetComponent<Team>();
    }

    public bool AddImpulse(Vector3 Impulse, bool ChangeVel = true)
    {
        //Impulse
        Impulse = Mathf.Lerp(SpeedMultiplierClamp.y , SpeedMultiplierRemap.y, Mathf.InverseLerp(SpeedMultiplierClamp.x, SpeedMultiplierRemap.x, Impulse.magnitude)) * Impulse.normalized;
        _rigidBody.AddForce(Impulse, ChangeVel? ForceMode.VelocityChange : ForceMode.Impulse);
        return true;
    }

    private void OnCollisionEnter(Collision other)
    {
        // other.gameObject.Interact(this.gameObject);
        if((_interactableLayerMask.value & (1 << other.gameObject.layer)) != 0) 
        {
            other.gameObject.Interact(gameObject);
        }

        if((_paddleLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            if(other.transform.parent.parent.gameObject.TryGetComponent(out Team team))
            {
                _team.OwnerId = team.OwnerId;
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpeedMultiplierOverTime = Mathf.Clamp(SpeedMultiplierOverTime + Time.deltaTime, SpeedMultiplierClamp.x, SpeedMultiplierClamp.y);   
    }

        [SerializeField] private float sphereRadius = 0.5f;
        [SerializeField] private float castDistance = 6f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float hoverHeight = 0.5f;

        void FixedUpdate()
        {
            SpeedMultiplierOverTime = Mathf.Clamp(SpeedMultiplierOverTime + Time.fixedDeltaTime, SpeedMultiplierClamp.x, SpeedMultiplierClamp.y); 
            Vector3 origin = transform.position + Vector3.up * 0.1f;

            if (Physics.SphereCast(origin, sphereRadius, Vector3.down, out RaycastHit hit, castDistance, groundLayer))
            {
                float dot = Vector3.Dot(Vector3.up, hit.normal);

                if (dot >= 0.25f) 
                {
                    Vector3 targetWorldPos = hit.point + hit.normal * hoverHeight;
                    if (TryGetComponent(out Rigidbody rb))
                    {
                        rb.position = targetWorldPos;

                        Vector3 horizontalVel = Vector3.ProjectOnPlane(rb.linearVelocity, hit.normal);
                        rb.linearVelocity = horizontalVel;
                    }
                }
            }
        }

    public bool Interact(GameObject Instigator, string Action = "")
    {
        if(Action == "Spike")
        {
            TryExecuteSpike(_rigidBody.linearVelocity, _rigidBody.linearVelocity.magnitude * 5, 0.5f);
            return true;
        }
        return false;
    }

    public bool Interact(GameObject Instigator, Action callback = null)
    {
        return false;
    }

    public List<string> GetInteractOptions(GameObject Instigator = null)
    {
        return new List<string>{"Spike"};
    }

    public void TryExecuteSpike(Vector3 shootDirection, float force, float freezeSecond)
    {
        if (isSpiking) return;
        StartCoroutine(SpikeRoutine(shootDirection, force, freezeSecond));
    }
    private bool isSpiking = false;
    public float SpikingCD = 1.0f;
    IEnumerator SpikeRoutine(Vector3 dir, float force, float freezeSecond)
    {
        isSpiking = true;

        bool useGravity = _rigidBody.useGravity;
        _rigidBody.useGravity = false;
        
        _rigidBody.linearVelocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        
        //Jitter
        float elapsed = 0;

        Vector3 originalLocalPos = transform.position; 
    
        while (elapsed < freezeSecond) 
        {
            transform.position = originalLocalPos + UnityEngine.Random.insideUnitSphere * 0.05f;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalLocalPos;

        //End of Jitter

        _rigidBody.useGravity = useGravity;

        Vector3 launchDir = dir.normalized;

        _rigidBody.AddForce(launchDir * force, ForceMode.Impulse);

        yield return new WaitForSeconds(SpikingCD);

        isSpiking = false;
    }
}
