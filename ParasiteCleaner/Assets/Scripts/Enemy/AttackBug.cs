using UnityEngine;
using System.Collections;

public class AttackBug : MonoBehaviour
{
    [Header("ATTACK")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float attackFreezeTime = 0.7f;

    private Rigidbody2D rb;
    private Animator anim;
    private Enemy enemy;

    private float attackTimer;
    private bool isAttacking;
    private bool playerInRange;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (!playerInRange || isAttacking) return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
            StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        // Congelar movimiento del Enemy
        if (enemy != null)
            enemy.canMove = false;

        // Congelar Rigidbody momentáneamente
        float originalXVelocity = rb.linearVelocity.x;
        rb.linearVelocity = Vector2.zero;

        // Animación de ataque con parámetro
        if (anim != null)
            anim.SetBool("Attack", true);

        // Instanciar proyectil
        if (projectilePrefab != null && shootPoint != null && enemy != null)
        {
            GameObject projGO = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            ProjectileBug proj = projGO.GetComponent<ProjectileBug>();
            if (proj != null)
            {
                Vector2 dir = enemy.isFacingRight ? Vector2.right : Vector2.left;
                proj.SetDirection(dir);
            }
        }

        // Esperar mientras está congelado
        yield return new WaitForSeconds(attackFreezeTime);

        // Restaurar movimiento
        rb.linearVelocity = new Vector2(originalXVelocity, rb.linearVelocity.y);
        if (enemy != null)
            enemy.canMove = true;

        // Terminar animación de ataque
        if (anim != null)
            anim.SetBool("Attack", false);

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
