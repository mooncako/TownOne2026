using UnityEngine;

public class MainMenuPlayerCursor : MonoBehaviour
{
    public enum CursorLocation
    {
        Origin,
        Heaven,
        Hell
    }

    private Vector3 _ogLocalPos;
    public bool IsInOgPos = true;
    public CursorLocation CurrentLocation { get; private set; } = CursorLocation.Origin;
    public bool IsOnHeaven => CurrentLocation == CursorLocation.Heaven;
    public bool IsOnHell => CurrentLocation == CursorLocation.Hell;

    void Awake()
    {
        _ogLocalPos = transform.localPosition;
    }

    public void Move(Vector3 pos, CursorLocation location)
    {
        transform.localPosition = pos;
        IsInOgPos = false;
        CurrentLocation = location;
    }

    public void ResetPosition()
    {
        transform.localPosition = _ogLocalPos;
        IsInOgPos = true;
        CurrentLocation = CursorLocation.Origin;
    }

}
