using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Jump Stats")]
    [SerializeField][Range(1, 2)] int maxJumps = 2;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float jumpHoldDuration = 0.5f;
    [SerializeField] private float jumpHoldForce = 5.0f;

    [Header("Movement")]
    [SerializeField] float speed = 10;

    private Rigidbody rb;

    private bool isJumpRequested;
    private bool isHoldingJump;
    private int consecutiveJumps = 0;

    private float jumpHoldTimer = 0;

    private Vector3 transformInput;
    private Vector3 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        InputReader.MoveEvent += OnMove;

        InputReader.JumpEvent += OnJump;
        InputReader.JumpHoldEvent += OnJumpHold;
        InputReader.JumpReleaseEvent += OnJumpRelease;
    }


    private void OnDisable()
    {
        InputReader.MoveEvent -= OnMove;

        InputReader.JumpEvent -= OnJump;
        InputReader.JumpHoldEvent -= OnJumpHold;
        InputReader.JumpReleaseEvent -= OnJumpRelease;
    }

    private void FixedUpdate()
    {
        transformInput = rb.transform.TransformDirection(moveInput.normalized) * speed;

        rb.AddForce(transformInput, ForceMode.Force);

        if (isJumpRequested)
        {
            isJumpRequested = false;
            jumpHoldTimer = 0;

            if (consecutiveJumps < maxJumps)
            {
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
        isHoldingJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        consecutiveJumps = 0;
    }
}
