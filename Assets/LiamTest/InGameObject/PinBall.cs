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
        _rigidBody.AddForce(Impulse, ForceMode.Impulse);
        return true;
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.Interact(this.gameObject);
        if((_interactableLayerMask.value & (1 << other.gameObject.layer)) != 0) 
        {
            other.gameObject.Interact(gameObject);
        }

        if((_paddleLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            if(other.gameObject.TryGetComponent(out Team team))
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
}
