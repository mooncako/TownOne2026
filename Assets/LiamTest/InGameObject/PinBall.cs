using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
public class PinBall : MonoBehaviour, IPhysics, IInteract
{
    public VisualEffect vfx;
    [SerializeField] private GameObject ClashVfx;
    [SerializeField] private GameObject ChargingVfx;
    [SerializeField, BoxGroup("References")] private Rigidbody _rigidBody;
    [SerializeField, BoxGroup("References")] private Team _team;
    [SerializeField, BoxGroup("Settings")] private LayerMask _interactableLayerMask;
    [SerializeField, BoxGroup("Settings")] private LayerMask _paddleLayerMask;

    public float HitTimes = 0.0f;
    protected Vector2 SpeedMultiplierClamp = new Vector2(0.0f, 25.0f);
    protected Vector2 SpeedMultiplierRemap = new Vector2(1.0f, 3.0f);


    private void OnValidate()
    {
        if(_rigidBody == null) _rigidBody = GetComponent<Rigidbody>();
        if(_team == null) _team = GetComponent<Team>();
    }

    public bool AddImpulse(Vector3 Impulse, bool ChangeVel = true)
    {
        //Impulse
        Impulse = Mathf.Lerp(SpeedMultiplierClamp.y , SpeedMultiplierRemap.y, Mathf.InverseLerp(SpeedMultiplierClamp.x, SpeedMultiplierRemap.x, HitTimes)) * Impulse;
        Impulse *= 0.45f;
        _rigidBody.AddForce(Impulse, ChangeVel? ForceMode.VelocityChange : ForceMode.Impulse);
        HitTimes = Mathf.Clamp(HitTimes+1.0f, SpeedMultiplierClamp.x, SpeedMultiplierClamp.y);
        //_rigidBody.AddForce(Impulse, ForceMode.VelocityChange);
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

        if(other.gameObject.TryGetComponent<PinBall>(out PinBall ball) && ClashVfx)
        {
            
        }

        if(ClashVfx)
        {
            GameObject effect = Instantiate(ClashVfx, other.contacts[0].point, Quaternion.identity);
            Destroy(effect, 0.8f);
        }
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        [SerializeField] private float sphereRadius = 0.5f;
        [SerializeField] private float castDistance = 6f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float hoverHeight = 0.5f;

        private Vector3 UpDir = new Vector3(0,1,0);
        void FixedUpdate()
        {
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
                        UpDir = hit.normal;
                        rb.linearVelocity = horizontalVel;
                    }
                }
            }

            if (_rigidBody != null && vfx != null)
            {
                vfx.SetVector3("TargetVelocity", _rigidBody.linearVelocity);
            }
        }

    public bool Interact(GameObject Instigator, string Action = "")
    {
        if(Action == "Spike")
        {
            TryExecuteSpike(_rigidBody.linearVelocity, _rigidBody.linearVelocity.magnitude * 5, 0.2f);
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
        if(ChargingVfx)
        {
            GameObject effect = Instantiate(ChargingVfx, this.gameObject.transform.position, Quaternion.identity);
            Destroy(effect, 0.8f);  
        }

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
            Vector3 horizontalVel = Vector3.ProjectOnPlane(dir.normalized, UpDir);
            //originalLocalPos +=elapsed * dir.normalized * force * 0.01f;
            transform.position = originalLocalPos + UnityEngine.Random.insideUnitSphere * 0.1f;
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

    void LateUpdate()
    {
        vfx.transform.rotation = Quaternion.identity; 
    }

}
