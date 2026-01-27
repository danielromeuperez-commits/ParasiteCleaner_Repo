using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Config")]
    [SerializeField] float speed;
    public bool isFacingRight;

    void Update()
    {
        ProjectileMove();
    }

    void ProjectileMove()
    {
        // Mover en mundo global según la dirección
        if (isFacingRight)
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        else
            transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "CamConfiner" && collision.gameObject.tag != null)
        {
            gameObject.SetActive(false);
        }
    }
}
