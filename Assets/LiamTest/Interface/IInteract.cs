using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    bool Interact(GameObject Instigator, string Action = "");
    List<string> GetInteractOptions(GameObject Instigator = null);
}