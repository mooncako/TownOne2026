using Unity.VisualScripting;
using UnityEngine;

public class BallRespawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector3 respawnLocation = new Vector3(0,2.0f,0);
    public float force = 2.0f;

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent<PinBall>(out PinBall ball))
        {
            other.gameObject.transform.position = respawnLocation;
            if(ball.TryGetComponent<Rigidbody>(out Rigidbody RD))
            {
             RD.linearVelocity = Vector3.zero;   
            }
            ball.HitTimes = 0.0f;
            Vector3 RandomDir = Vector3.ProjectOnPlane(Random.onUnitSphere, Vector3.up).normalized;
            ball.AddImpulse(RandomDir * force);
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
