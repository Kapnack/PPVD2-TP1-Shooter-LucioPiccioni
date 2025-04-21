using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private float xRatation = 0.0f;

    [Header("Mouse Sensitivity")]
    [SerializeField] private float xSensitivity = 30.0f;
    [SerializeField] private float ySensitivity = 30.0f;

    [Header("Camera Y Clamp")]
    [SerializeField][Range(-90, 0)] private float HighClamp = -80;
    [SerializeField][Range(0, 90)] private float lowClamp = 80;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InputReader.lookEvent += OnMouseMove;
    }

    private void OnDisable()
    {
        InputReader.lookEvent -= OnMouseMove;
    }

    private void OnMouseMove(Vector2 direction)
    {
        xRatation -= (direction.y * Time.deltaTime) * ySensitivity;
        xRatation = Mathf.Clamp(xRatation, HighClamp, lowClamp);

        cam.transform.localRotation = Quaternion.Euler(xRatation, 0, 0);

        rb.transform.Rotate(Vector3.up * (direction.x * Time.deltaTime) * xSensitivity);
    }
}
