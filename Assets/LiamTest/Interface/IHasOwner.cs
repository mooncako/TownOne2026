using UnityEngine;

public interface IHasOwner
{
    PlayerStateSO GetOwner();
    bool SetOwner(PlayerStateSO newOwner);
    bool CanPossess();
}
