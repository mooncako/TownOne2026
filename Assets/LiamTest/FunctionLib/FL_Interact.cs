using System.Collections.Generic;
using UnityEngine;

public static class FL_Interact
{
    public static bool GetInteractOptions(this GameObject obj, out List<string> options, GameObject Instigator = null)
    {
        options = new List<string>();

        IInteract[] comps = obj.GetComponentsInChildren<IInteract>();
        if(comps == null || comps.Length <= 0) return false;
        foreach(IInteract comp in comps)
        {
            List<string> optionsNames = comp.GetInteractOptions(Instigator);
            options.AddRange(optionsNames);
        }
        
        return true;
    }

    public static bool Interact(this GameObject obj, GameObject Instigator = null, string ActionOption = "")
    {

        IInteract[] comps = obj.GetComponentsInChildren<IInteract>();
        if(comps == null || comps.Length <= 0) return false;
        foreach(IInteract comp in comps)
        {
            List<string> optionsNames = comp.GetInteractOptions(Instigator);
            if(optionsNames.Contains(ActionOption))
            {
                return comp.Interact(Instigator, ActionOption);
            }
        }
        return false;
    }
}
