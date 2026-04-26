using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PinBall : MonoBehaviour, IPhysics
{
    [SerializeField, BoxGroup("References")] private Rigidbody _rigidBody;
    [SerializeField, BoxGroup("References")] private Team _team;
    [SerializeField, BoxGroup("Settings")] private LayerMask _interactableLayerMask;
    [SerializeField, BoxGroup("Settings")] private LayerMask _paddleLayerMask;


    private void OnValidate()
    {
        if(_rigidBody == null) _rigidBody = GetComponent<Rigidbody>();
        if(_team == null) _team = GetComponent<Team>();
    }

    public bool AddImpulse(Vector3 Impulse, bool ChangeVel = true)
    {
        //Impulse
        
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
        
    }

        [SerializeField] private float sphereRadius = 0.5f;
        [SerializeField] private float castDistance = 6f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float hoverHeight = 0.5f;

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
                        rb.linearVelocity = horizontalVel;
                    }
                }
            }
        }
}
