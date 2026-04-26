using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Paddle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float force = 20.0f;

    float spikeCD = 8.0f;
    private bool isSpikeCD = false; 

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent<IPhysics>(out IPhysics comp))
        {
            Vector3 normal = other.GetContact(0).normal * -1;
            normal = Vector3.ProjectOnPlane(normal, Vector3.up);
            comp.AddImpulse(normal * force);
        }

        TryToSpike(other.gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool TryToSpike(GameObject Target)
    {
        //Ready to spike
        if(!Target || !isSpikeCD)
        {
            if(Target.GetInteractOptions(out List<string> options, this.gameObject))
            {
                if(options.Contains("Spike"))
                {
                    if(Target.Interact(this.gameObject, "Spike"));
                    {
                        isSpikeCD = true;
                        StartCoroutine(StartSpikeCD());
                        return true;
                    }
                }
            }
        }
        return false;
    }

    IEnumerator StartSpikeCD()
    {
        yield return new WaitForSeconds(spikeCD);
        isSpikeCD = false;
    }
}
