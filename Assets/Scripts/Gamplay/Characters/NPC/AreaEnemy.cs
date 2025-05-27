using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AreaEnemy : Enemy
{
    [SerializeField] private float viewDistance = 100;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private float explodeRange = 1.5f;
    [SerializeField] private float explosionDamage = 200;
    [SerializeField] private GameObject explosionEffect;

    private bool hasExploded = false;
    private Vector3 initialPosition;
    private float lostPlayerTimer = 0f;
    private bool playerInSight = false;

    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();

        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (iPlayer == null)
        {
            Debug.LogWarning("iPlayer es null");
            return;
        }

        Vector3 playerPos = iPlayer.getPos();
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        if (distanceToPlayer < viewDistance)
        {
            playerInSight = true;
            lostPlayerTimer = 0f;

            Vector3 dirToPlayer = playerPos - transform.position;
            dirToPlayer.y = 0;

            if (dirToPlayer != Vector3.zero)
                RotateTowardsDirection(dirToPlayer.normalized);

            Vector3 moveDir = rb.rotation * Vector3.forward;
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
        else if (playerInSight)
        {
            lostPlayerTimer += Time.fixedDeltaTime;
            if (lostPlayerTimer >= 3f)
                playerInSight = false;
        }

        if (!playerInSight && lostPlayerTimer >= 3f)
        {
            Vector3 dirBack = initialPosition - transform.position;
            dirBack.y = 0;

            if (dirBack.magnitude > 0.1f)
            {
                RotateTowardsDirection(dirBack.normalized);
                Vector3 moveDir = rb.rotation * Vector3.forward;
                rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
            }
        }

        float horizontalDistance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(playerPos.x, playerPos.z)
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

        Vector3 euler = newRotation.eulerAngles;
        newRotation = Quaternion.Euler(0, euler.y, 0);

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
            Debug.Log("<color=red><b>[Enemy]</b> ¡BOOM! Daño al jugador</color>");
            iPlayer.ReciveDamage(explosionDamage);
        }

        DieAndNotify();
    }

    protected override void OnDead()
    {
        DieAndNotify();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
