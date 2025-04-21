using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float fireForce = 1000;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire() => rb.AddForce(transform.forward * fireForce, ForceMode.Impulse);
}
