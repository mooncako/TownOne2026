using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float launchForce = 1f;
    [SerializeField] private Vector3 launchDirection = Vector3.forward;
    private Vector3 startPosition;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Keyboard.current.f10Key.isPressed)
            ResetBall();
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
        Launch();
    }

    public void Launch()
    {
        rb.AddForce(launchDirection* launchForce, ForceMode.Impulse);
    }
}