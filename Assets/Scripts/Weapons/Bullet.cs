using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    private int damage = 0;

    [SerializeField] private float fireForce = 1000;

    [SerializeField] private const float durationTime = 10.0f;

    private float createdTime = 0;

    private Rigidbody rb;

    void Awake()
    {
        createdTime = createdTime = Time.time;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    public void SetUpDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetDamege(int damage) => this.damage = damage;
    public void Fire() => rb.AddForce(transform.forward * fireForce, ForceMode.Impulse);

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
            enemy.OnDamage(damage);

        Destroy(gameObject);
    }
}
