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

    [Header("Health")]
    public int healthPoints;

    [HideInInspector] public bool canMove = true;

    private bool isGrounded;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Check si toca el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Detenerse si no puede moverse
        if (!canMove)
        {
            anim.SetBool("Walk", false);
            return;
        }

        // Mover siempre en la dirección actual
        float dir = isFacingRight ? 1f : -1f;
        transform.position += Vector3.right * dir * speed * Time.deltaTime;

        // Flip automático al caer del suelo
        if (!isGrounded)
        {
            Flip();
        }

        // Actualizar parámetro de animación
        anim.SetBool("Walk", true);
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
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
