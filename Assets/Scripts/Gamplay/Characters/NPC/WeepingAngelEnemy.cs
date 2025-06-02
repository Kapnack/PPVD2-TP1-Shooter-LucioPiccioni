using UnityEngine;

public class WeepingAngelEnemy : Enemy
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float explodeRange = 3f;
    [SerializeField] private float explosionDamage = 200;
    [SerializeField] private GameObject explosionEffect;

    private bool hasExploded = false;

    private Rigidbody rb;
    private Vector3 initialPosition;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (iPlayer == null) return;

        Vector3 playerPos = iPlayer.getPos();
        Vector3 dirToPlayer = playerPos - transform.position;
        dirToPlayer.y = 0;

        if (!iPlayer.IsLookingAt(transform.position))
        {
            if (dirToPlayer.magnitude > 0.1f)
            {
                RotateTowardsDirection(dirToPlayer.normalized);
                Vector3 moveDir = rb.rotation * Vector3.forward;
                rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
            }
        }

        float horizontalDistance = Vector3.Distance(
            transform.position,
            playerPos
        );

        if (!hasExploded && horizontalDistance < explodeRange)
        {
            Explode(playerPos);
        }
    }

    private void RotateTowardsDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);
        rb.MoveRotation(newRotation);
    }

    private void Explode(Vector3 playerPos)
    {
        hasExploded = true;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        float dist = Vector3.Distance(transform.position, playerPos);
        if (dist < explodeRange)
        {
            Debug.Log("<color=red><b>[EnemySneaky]</b> ¡BOOM! Daño al jugador</color>");
            iPlayer.ReciveDamage(explosionDamage);
        }

        DieAndNotify();
    }

    protected override void OnDead()
    {
        DieAndNotify();
    }
}
