using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MenuPlayer : MonoBehaviour
{
    public PlayerId Id;

    private void OnNavigation(InputValue value)
    {
        Vector2 navigation = value.Get<Vector2>();
        MenuPlayerInputEvent.Trigger(navigation, Id);
    }

    private void OnSubmit(InputValue value)
    {
        if (value.isPressed)
        {
            MenuPlayerSubmitEvent.Trigger();
        }
    }
}
