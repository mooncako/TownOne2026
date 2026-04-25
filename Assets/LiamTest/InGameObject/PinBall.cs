using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PinBall : MonoBehaviour, IPhysics
{
    public bool AddImpulse(Vector3 Impulse, bool ChangeVel = true)
    {
        //Impulse
        return true;
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.Interact(this.gameObject);
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
