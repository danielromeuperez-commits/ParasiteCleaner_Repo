using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Config")]
    [SerializeField] float speed;
    public bool isFacingRight;
    public int damage;

    private SpriteRenderer projectileRend;

    private void Awake()
    {
        projectileRend = GetComponent<SpriteRenderer>();



        // Desactivar automáticamente después de 10 segundos
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        // Mover el proyectil
        float moveDirection = isFacingRight ? 1f : -1f;
        transform.Translate(Vector3.right * moveDirection * speed * Time.deltaTime);

        // Flip del sprite según isFacingRight
        Vector3 localScale = transform.localScale;
        localScale.x = isFacingRight ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Golpea enemigo
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            gameObject.SetActive(false);
            return;
        }

        // Golpea boss
        Boss boss = collision.GetComponent<Boss>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            gameObject.SetActive(false);
            return;
        }
    }
}
