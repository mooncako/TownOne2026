using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bouncer : MonoBehaviour
{
    public float force = 10;
    public bool EnlargeOnHit = false;

    private float sizeTick = 1.0f;
    private Vector3 OriginalSize = new Vector3(1,1,1);
    private void Start()
    {
        OriginalSize = this.gameObject.transform.localScale;
    }
    private void OnCollisionEnter(Collision other)
    {
        Vector3 normal = other.GetContact(0).normal * -1;
        normal = Vector3.ProjectOnPlane(normal, Vector3.up);
        other.gameObject.AddImpulse(normal * force, ForceMode.Impulse);
        sizeTick = 1.15f;
    }

    private void Update()
    {
        if(sizeTick > 1.0f)sizeTick = Mathf.Max(sizeTick-Time.deltaTime, 1.0f);
        this.gameObject.transform.localScale = OriginalSize * sizeTick;
    }
}
