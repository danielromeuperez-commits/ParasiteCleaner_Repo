using UnityEngine;

public class ProjectileBug : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] float lifeTime = 2f;
    [SerializeField] int damage = 1;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        AudioManager.Instance.PlaySFX(11);
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        dir = dir.normalized;
        rb.linearVelocity = dir * speed; 

        Vector3 localScale = transform.localScale;
        localScale.x = dir.x < 0 ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.playerHealth -= damage;

            Destroy(gameObject);
        }
    }
}
