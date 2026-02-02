using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float speed = 5f;
    [SerializeField] public float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.35f;
    [SerializeField] LayerMask groundLayer;

    [Header("Shoot")]
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootCooldown = 0.5f;

    Rigidbody2D rb;
    Animator anim;

    Vector2 moveInput;
    bool isFacingRight = true;

    bool isGrounded;
    bool jumpLocked;
    bool canShoot = true;
    bool isDead;

    public bool IsGrounded => isGrounded;
    public bool IsFacingRight => isFacingRight;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // ESCUCHA AL GAMEMANAGER
        if (!isDead && GameManager.Instance != null && GameManager.Instance.IsPlayerDead)
        {
            Die();
        }

        if (isDead) return;

        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("Walk", Mathf.Abs(moveInput.x) > 0.1f);

        if (moveInput.x > 0 && !isFacingRight) Flip();
        else if (moveInput.x < 0 && isFacingRight) Flip();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded)
            jumpLocked = false;
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
        if (!isGrounded || jumpLocked || isDead) return;

        jumpLocked = true;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // ================= SHOOT =================
    void Shoot()
    {
        if (!canShoot || !isGrounded || isDead) return;

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

    // ================= INPUT =================
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (isDead) return;
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
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        anim.SetTrigger("Death");
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        GameManager.Instance.OnPlayerDied();
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
