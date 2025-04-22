using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    private int damage = 12;
    private float createdTime = 0;
    [SerializeField] private float fireForce = 1000;

    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        createdTime = Time.time;
    }

    public void Fire() => rb.AddForce(transform.forward * fireForce, ForceMode.Impulse);

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
            enemy.OnDamage(damage);

        Destroy(gameObject);
    }
}
