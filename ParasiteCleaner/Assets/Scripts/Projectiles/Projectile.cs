using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Config")]
    [SerializeField] float speed;
    public bool isFacingRight;
    public int damage;

    void Update()
    {
        ProjectileMove();
    }

    void ProjectileMove()
    {
        if (isFacingRight)
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        else
            transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Hacer daño
        }

        // Desactivar el proyectil en cualquier caso
        gameObject.SetActive(false);
    }
}