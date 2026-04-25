using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Paddle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float force = 20.0f;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent<IPhysics>(out IPhysics comp))
        {
            Vector3 normal = other.GetContact(0).normal * -1;
            normal = Vector3.ProjectOnPlane(normal, Vector3.up);
            comp.AddImpulse(normal * force);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
