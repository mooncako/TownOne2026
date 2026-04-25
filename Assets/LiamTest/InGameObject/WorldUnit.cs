using System.Collections.Generic;
using UnityEngine;

public class WorldUnit : MonoBehaviour, IInteract
{
    public List<string> GetInteractOptions(GameObject Instigator = null)
    {
        return null;
    }

    public bool Interact(GameObject Instigator, string Action = "")
    {
        //We could use interace TryGetComponent<Interface>
        if(Instigator.TryGetComponent<PinBall>(out PinBall pinball))
        {
            //Dosomthing
            //ball Bouncing off
            //Loss health
            return true;   
        }
        return false;
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
