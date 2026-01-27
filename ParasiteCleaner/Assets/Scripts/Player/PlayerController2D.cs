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
    [SerializeField] bool isFacingRight;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    [Header("Shoot config")]
    [SerializeField] GameObject Projectile;
    [SerializeField] Transform Shootpoint;
    [SerializeField] float shootPoint;
    [SerializeField] bool canShoot;

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
        if (moveInput.x > 0 && !isFacingRight) Flip();
        if (moveInput.x < 0 && isFacingRight) Flip();
    }  
        
    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        playerRb.linearVelocity = new Vector2(moveInput.x * speed, playerRb.linearVelocity.y);
    }

    void Flip()
    {
        Vector3 currentScale = transform.localScale; //Almacén temporal de la escala del objeto
        currentScale.x *= -1; //Invertir el valor en X
        transform.localScale = currentScale; //Le devolvemos la escala al objeto con el valor en X inverso
        isFacingRight = !isFacingRight; //Decirle al bool que cambie al valor contrario
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }


    #endregion
}
