using UnityEngine;

public class ProjectileBug : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] float lifeTime = 2f;
    [SerializeField] int damage = 1;

    Vector2 direction;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.isKinematic = false; // para poder usar velocity
        rb.gravityScale = 0;

        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        rb.linearVelocity = direction * speed;

        // Flip visual
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (dir.x >= 0 ? 1 : -1);
        transform.localScale = scale;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Quitar vida desde el GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerHealth -= damage;
            }

            Destroy(gameObject);
        }
    }
}
