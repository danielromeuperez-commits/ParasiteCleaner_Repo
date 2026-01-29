using UnityEngine;
using System.Collections;

public class AttackBug : MonoBehaviour
{
    [Header("ATTACK")]
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float attackFreezeTime = 0.7f;

    Rigidbody2D rb;

    float attackTimer;
    bool isAttacking;
    bool playerInRange;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!playerInRange || isAttacking) return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
            StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        // Freeze en X
        float originalXVelocity = rb.linearVelocity.x;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        // Instancia del proyectil
        if (projectilePrefab != null && shootPoint != null)
            Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(attackFreezeTime);

        rb.linearVelocity = new Vector2(originalXVelocity, rb.linearVelocity.y);

        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
