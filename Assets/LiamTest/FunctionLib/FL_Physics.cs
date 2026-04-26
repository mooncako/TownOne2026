using UnityEngine;

public static class FL_Physics
{
    public static bool AddImpulse(this GameObject obj, Vector3 Force, ForceMode ForceMode = ForceMode.VelocityChange)
    {
        if(obj.TryGetComponent<IPhysics>(out IPhysics comp))
        {
            //Todo Update Interface Physics forcemode
            bool temp = ForceMode == ForceMode.VelocityChange;
            comp.AddImpulse(Force, temp);
            return true;
        }
        return false;
    }
}
