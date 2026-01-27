using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement & Jump config")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] bool isGrounded;
    [SerializeField] public bool isFacingRight;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [Header("Coyote time")]
    [SerializeField] float coyoteTime =0.2f;
    [SerializeField] float coyoteTimeCounter;
    [Header("Shoot config")]
    [SerializeField] GameObject Projectile;
    [SerializeField] Transform Shootpoint;
    [SerializeField] bool canShoot;
    [SerializeField] float shootCooldown; 
    //Refs generales
    Rigidbody2D playerRb;
    PlayerInput input;
    Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        canShoot = true;
    }

    void Start()
    {
        isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Coyote time
        if (isGrounded)
            coyoteTimeCounter = coyoteTime; // reinicia cuando estás en el suelo
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Flip
        if (moveInput.x > 0 && !isFacingRight) Flip();
        if (moveInput.x < 0 && isFacingRight) Flip();
    }  
        
    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        playerRb.linearVelocity = new Vector2(moveInput.x * speed, playerRb.linearVelocity.y);
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    void Shoot()
    {
        if (canShoot)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    IEnumerator ShootCoroutine()
    {
        canShoot = false; // Evita disparar otra vez mientras la corrutina está activa

        int shots = 3; // Numero de proyectiles por disparo
        float delay = 0.2f; // Tiempo entre cada proyectil (ajusta según necesites)

        for (int i = 0; i < shots; i++)
        {
            // Instanciamos el proyectil
            GameObject proj = Instantiate(Projectile, Shootpoint.position, Quaternion.identity);

            // Ajustamos la dirección del proyectil
            Projectile projScript = proj.GetComponent<Projectile>();
            projScript.isFacingRight = isFacingRight;

            yield return new WaitForSeconds(delay);
        }

        // Esperamos el cooldown antes de poder disparar de nuevo
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }


    void resetShoot()
    {
        canShoot = true;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && coyoteTimeCounter > 0f)
        {
            Jump();                   // Aplica la fuerza de salto
            coyoteTimeCounter = 0f;    // Evita saltos dobles usando coyote time
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && canShoot) Shoot();
    }
    #endregion
}
