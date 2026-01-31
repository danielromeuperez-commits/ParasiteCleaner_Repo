using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Shoot")]
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootCooldown = 0.5f;

    Rigidbody2D rb;
    Animator anim;
    Vector2 moveInput;
    bool canShoot = true;
    bool isFacingRight = true;
    bool isGrounded;
    bool jumpLocked;
    bool isDead = false; // NUEVO: flag para controlar muerte

    // ================= PROPIEDADES PÚBLICAS =================
    public bool IsFacingRight => isFacingRight;
    public bool IsGrounded => isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Revisar si el jugador murió
        if (!isDead && GameManager.Instance.IsPlayerDead)
        {
            Die();
        }

        // Ground check
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded) jumpLocked = false;

        // Animator
        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("Walk", moveInput.x != 0);

        // Flip visual
        if (moveInput.x > 0 && !isFacingRight) Flip();
        else if (moveInput.x < 0 && isFacingRight) Flip();
    }

    void FixedUpdate()
    {
        if (!isDead) // Solo mover si no está muerto
            rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ================= JUMP =================
    void Jump()
    {
        if (!isGrounded || jumpLocked || isDead) // No saltar si murió
            return;
        jumpLocked = true;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // ================= SHOOT =================
    void Shoot()
    {
        if (!canShoot || !isGrounded || isDead) return; // No disparar si murió
        anim.SetBool("IsAttacking", true);
        anim.SetTrigger("Shoot");
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        canShoot = false;
        for (int i = 0; i < 3; i++)
        {
            GameObject proj = Instantiate(projectile, shootPoint.position, Quaternion.identity);
            proj.GetComponent<Projectile>().isFacingRight = isFacingRight;
            yield return new WaitForSeconds(0.2f);
        }
        anim.SetBool("IsAttacking", false);
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    // ================= INPUT SYSTEM =================
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!isDead) // No mover si murió
            moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Jump();
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Shoot();
    }

    // ================= DEATH =================
    void Die()
    {
        isDead = true;
        anim.SetTrigger("Death");
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        // Opcional: bloquear este script para que no se ejecute nada más
        // this.enabled = false;
    }
}
