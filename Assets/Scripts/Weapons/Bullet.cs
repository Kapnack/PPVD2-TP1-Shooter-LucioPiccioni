using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    private int damage = 0;

    [SerializeField] private float fireForce = 1000;

    [SerializeField] private const float durationTime = 2;

    private float createdTime = 0;

    private Rigidbody rb;

    private Characters owner;

    void Awake()
    {
        createdTime = Time.time;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Time.time > createdTime + durationTime)
            Destroy(gameObject);
    }

    public void SetOwner(Characters owner) => this.owner = owner;
    public void SetUpDamage(int damage) => this.damage = damage;

    public void Fire() => rb.AddForce(transform.forward * fireForce, ForceMode.Impulse);

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
            if (enemy.IsDeadAfterDamage(damage) && owner is Player player)
                player.AddKill();



        Destroy(gameObject);
    }
}
