using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rangeX = 1f;

    [Header("Flip")]
    [SerializeField] private float flipSpeed = 10f;
    [SerializeField] private float flipAngle = 15f;

    private Rigidbody rb;

    private float inputAxis;

    private float flipProgress;
    private float startingAngle = 0f;
    private float targetAngle = 0f;
    private float GetCurrentAngle()
    {
        float angle = rb.rotation.eulerAngles.y;
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void Update()
    {
        if (Keyboard.current.upArrowKey.isPressed) inputAxis = 1f;
        else if (Keyboard.current.downArrowKey.isPressed) inputAxis = -1f;
        else inputAxis = 0f;

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame) SetFlip(1f);
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame) SetFlip(-1f);
        else if (Keyboard.current.rightArrowKey.wasReleasedThisFrame)
        {
            if (Keyboard.current.leftArrowKey.isPressed) SetFlip(-1f);
            else SetFlip(0f);
        }
        else if (Keyboard.current.leftArrowKey.wasReleasedThisFrame)
        {
            if (Keyboard.current.rightArrowKey.isPressed) SetFlip(1f);
            else SetFlip(0f);
        }
    }

    void FixedUpdate()
    {
        // Movement
        float targetX = Mathf.Clamp(rb.position.x + inputAxis * moveSpeed * Time.fixedDeltaTime, -rangeX, rangeX);
        rb.MovePosition(new Vector3(targetX, rb.position.y, rb.position.z));

        // Flip
        if (flipProgress > 0f)
        {
            flipProgress -= Time.fixedDeltaTime * flipSpeed;
            float t = Mathf.SmoothStep(0f, 1f, 1f - flipProgress);
            float angle = Mathf.LerpUnclamped(startingAngle, targetAngle, t);
            rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
        }
    }

    private void SetFlip(float direction)
    {
        startingAngle = GetCurrentAngle();
        targetAngle = flipAngle * direction;
        flipProgress = 1f;
    }
}