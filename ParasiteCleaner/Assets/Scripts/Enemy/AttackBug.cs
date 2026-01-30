using UnityEngine;
using System.Collections;

public class AttackBug : MonoBehaviour
{
    [Header("ATTACK")]
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float attackFreezeTime = 0.7f;

    [Header("DIRECTION")]
    [SerializeField] bool facingRight = true; // true = derecha, false = izquierda

    Rigidbody2D rb;
    Animator anim; // Opcional: si tienes animaciones

    float attackTimer;
    bool isAttacking;
    bool playerInRange;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

        // Congelar movimiento horizontal
        float originalXVelocity = rb.linearVelocity.x;
        rb.linearVelocity = Vector2.zero;

        // Animación de ataque
        if (anim != null)
            anim.SetTrigger("Attack");

        // Instanciar proyectil
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projGO = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            ProjectileBug proj = projGO.GetComponent<ProjectileBug>();

            if (proj != null)
            {
                // Dirección basada en la rotación del shootPoint
                Vector2 dir = shootPoint.right.normalized;
                proj.SetDirection(dir);
            }
        }

        // Esperar mientras está congelado
        yield return new WaitForSeconds(attackFreezeTime);

        // Restaurar movimiento horizontal
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

    // Función para girar el enemigo
    public void Flip()
    {
        facingRight = !facingRight;

        // Mantener la escala del enemigo positiva
        Vector3 scale = transform.localScale;
        scale.x = 1f;
        transform.localScale = scale;

        // Rotar el shootPoint 180° en Y
        if (shootPoint != null)
        {
            Vector3 shootEuler = shootPoint.localEulerAngles;
            shootEuler.y += 180f;
            shootPoint.localEulerAngles = shootEuler;
        }
    }
}
