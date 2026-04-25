using Unity.VisualScripting.InputSystem;
using UnityEngine;

public static class FL_Owner
{
    public static PlayerStateSO GetOwner(GameObject obj)
    {
        if(obj.TryGetComponent(out IHasOwner comp))
        {
            return comp.GetOwner();
        }
        return null;
    }
}
