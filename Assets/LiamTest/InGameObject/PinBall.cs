using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PinBall : MonoBehaviour, IPhysics
{
    [SerializeField, BoxGroup("References")] private Rigidbody _rigidBody;
    [SerializeField, BoxGroup("References")] private Team _team;
    [SerializeField, BoxGroup("Settings")] private LayerMask _interactableLayerMask;


    private void OnValidate()
    {
        if(_rigidBody == null) _rigidBody = GetComponent<Rigidbody>();
    }

    public bool AddImpulse(Vector3 Impulse, bool ChangeVel = true)
    {
        //Impulse
        
        _rigidBody.AddForce(Impulse, ChangeVel? ForceMode.VelocityChange : ForceMode.Impulse);
        return true;
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.Interact(this.gameObject);
        if((_interactableLayerMask.value & (1 << other.gameObject.layer)) != 0) 
        {
            other.gameObject.Interact(gameObject);
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
