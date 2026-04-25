using UnityEngine;

public interface IPhysics
{
    public bool AddImpulse(Vector3 Impulse, bool ChangeVel = true);
}
