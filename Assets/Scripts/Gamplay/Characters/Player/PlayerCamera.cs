using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    InputReader inputReader = null;

    [SerializeField] private Camera cam;
    private float xRatation = 0.0f;

    [Header("Mouse Sensitivity")]
    [SerializeField] private float xSensitivity = 30.0f;
    [SerializeField] private float ySensitivity = 30.0f;

    [Header("Camera Y Clamp")]
    [SerializeField][Range(-90, 0)] private float HighClamp = -80;
    [SerializeField][Range(0, 90)] private float lowClamp = 80;

    bool isHoldingMoving = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (ServiceProvider.TryGetService<InputReader>(out var inputReader))
            this.inputReader = inputReader;
    }

    private void OnEnable()
    {
        inputReader.lookEvent += OnMoveCamera;
        inputReader.lookEventHolding += OnHoldingLook;
        inputReader.lookEventCanceled += OnCanceledLook;
    }

    private void OnDisable()
    {
        if (ServiceProvider.TryGetService<InputReader>(out var inputReader))
        {
            inputReader.lookEvent -= OnMoveCamera;
            inputReader.lookEventHolding -= OnHoldingLook;
            inputReader.lookEventCanceled -= OnCanceledLook;
        }
    }

    private void Update()
    {
        if (isHoldingMoving)
        {
            Vector2 lookInput = inputReader.GetLookVector2();
            OnMoveCamera(lookInput);
        }
    }

    private void OnMoveCamera(Vector2 direction)
    {
        float mouseX = direction.x * xSensitivity * Time.deltaTime;
        float mouseY = direction.y * ySensitivity * Time.deltaTime;

        xRatation -= mouseY;
        xRatation = Mathf.Clamp(xRatation, HighClamp, lowClamp);

        cam.transform.localRotation = Quaternion.Euler(xRatation, 0.0f, 0.0f);

        rb.transform.Rotate(Vector3.up * mouseX);

        Quaternion yawRotation = Quaternion.Euler(0f, mouseX, 0f);
        rb.MoveRotation(rb.rotation * yawRotation);
    }

    private void OnHoldingLook(Vector2 direction)
    {
        isHoldingMoving = true;
    }

    private void OnCanceledLook(Vector2 direction)
    {
        isHoldingMoving = false;
    }
}
