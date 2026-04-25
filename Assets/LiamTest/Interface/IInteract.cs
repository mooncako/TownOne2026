using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    bool Interact(GameObject Instigator, string Action = "");
    bool Interact(GameObject Instigator, Action callback = null);
    List<string> GetInteractOptions(GameObject Instigator = null);
}