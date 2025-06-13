using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Jump Stats")]
    [SerializeField][Range(1, 2)] int maxJumps = 2;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float jumpHoldDuration = 0.5f;
    [SerializeField] private float jumpHoldForce = 5.0f;

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 40f;

    private Rigidbody rb;
    private IInputReader iInputReader;

    private bool isJumpRequested;
    private bool isHoldingJump;
    private int consecutiveJumps = 0;
    private float jumpHoldTimer = 0;

    private Vector3 moveInput;
    private Vector3 currentVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (ServiceProvider.TryGetService<IInputReader>(out var iInputReader))
            this.iInputReader = iInputReader;
    }

    private void OnEnable()
    {
        iInputReader.MoveEvent += OnMove;
        iInputReader.JumpEvent += OnJump;
        iInputReader.JumpHoldEvent += OnJumpHold;
        iInputReader.JumpReleaseEvent += OnJumpRelease;
    }

    private void OnDisable()
    {
        iInputReader.MoveEvent -= OnMove;
        iInputReader.JumpEvent -= OnJump;
        iInputReader.JumpHoldEvent -= OnJumpHold;
        iInputReader.JumpReleaseEvent -= OnJumpRelease;
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = transform.TransformDirection(moveInput.normalized) * maxSpeed;
        Vector3 currentHorizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 velocityDifference = targetVelocity - currentHorizontalVelocity;


        Vector3 force = Vector3.zero;
        if (moveInput.magnitude > 0.1f)
        {
            force = Vector3.ClampMagnitude(velocityDifference * acceleration, acceleration);
        }
        else
        {
            force = -currentHorizontalVelocity.normalized * deceleration;
            if (currentHorizontalVelocity.magnitude < 0.1f)
            {
                force = -currentHorizontalVelocity * deceleration;
            }
        }

        rb.AddForce(force, ForceMode.Force);

        if (isJumpRequested)
        {
            isJumpRequested = false;
            jumpHoldTimer = 0;

            if (consecutiveJumps < maxJumps)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                consecutiveJumps++; 
            }
        }

        if (isHoldingJump && jumpHoldTimer < jumpHoldDuration)
        {
            rb.AddForce(Vector3.up * jumpHoldForce, ForceMode.Force);
            jumpHoldTimer += Time.fixedDeltaTime;
        }
    }

    private void OnMove(Vector2 direction)
    {
        moveInput = new Vector3(direction.x, 0f, direction.y);
    }

    private void OnJump()
    {
        isJumpRequested = true;
    }

    private void OnJumpHold()
    {
        isHoldingJump = true;
    }

    private void OnJumpRelease()
    {
        isHoldingJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        consecutiveJumps = 0;
    }
}
