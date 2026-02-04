using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement config")]
    public float speed = 3f;
    public bool isFacingRight = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Wall Check")]
    public Transform wallCheck;
    public float wallCheckRadius = 0.2f;

    [Header("Health")]
    public int healthPoints;

    [Header("Death Effects")]
    public ParticleSystem deathParticles;

    [HideInInspector] public bool canMove = true;

    private bool isGrounded;
    private Animator anim;
    private bool isDead = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (!canMove)
        {
            anim.SetBool("Walk", false);
            return;
        }

        Patrol();

        float dir = isFacingRight ? 1f : -1f;
        transform.position += Vector3.right * dir * speed * Time.deltaTime;

        if (!isGrounded)
        {
            Flip();
        }

        anim.SetBool("Walk", true);
    }

    void Patrol()
    {
        if (wallCheck == null) return;

        bool isTouchingWall = Physics2D.OverlapCircle(
            wallCheck.position,
            wallCheckRadius,
            groundLayer
        );

        if (isTouchingWall)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        canMove = false;
        anim.SetBool("Walk", false);

        if (deathParticles != null)
        {
            deathParticles.Play();
        }
        AudioManager.Instance.PlaySFX(7);
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        enabled = false;
        Destroy(gameObject, 1.5f);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }
}
