using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bouncer : MonoBehaviour
{
    public float force = 10;
    private void OnCollisionEnter(Collision other)
    {
        Vector3 normal = other.GetContact(0).normal * -1;
        normal = Vector3.ProjectOnPlane(normal, Vector3.up);
        other.gameObject.AddImpulse(normal * force, ForceMode.Impulse);
    }
}
