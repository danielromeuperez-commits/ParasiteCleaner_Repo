using UnityEngine;

public class Health : MonoBehaviour
{
    public float vida = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerHealth += vida;

                // Limitar al máximo
                if (GameManager.Instance.playerHealth > GameManager.Instance.maxHealth)
                    GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
            }

            Destroy(gameObject); // opcional
        }
    }
}
