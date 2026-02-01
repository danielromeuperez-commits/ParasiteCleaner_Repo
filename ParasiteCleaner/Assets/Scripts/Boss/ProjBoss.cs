using UnityEngine;

public class ProjBoss : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] float speed = 6f;
    [SerializeField] float lifeTime = 3f;
    [SerializeField] int damage = 1;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        Destroy(gameObject, lifeTime);

        // Forzar flip en -X
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void SetDirection(Vector2 dir)
    {
        dir = dir.normalized;
        rb.linearVelocity = dir * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerHealth -= damage;
            }

            Destroy(gameObject);
        }
    }
}
