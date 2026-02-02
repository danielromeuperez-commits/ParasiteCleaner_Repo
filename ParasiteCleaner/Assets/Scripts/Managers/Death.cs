using UnityEngine;

public class Death : MonoBehaviour
{
    [Header("Settings")]
    public string playerTag = "Player"; // Tag del jugador

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (GameManager.Instance != null)
            {
                // Poner la vida a 0
                GameManager.Instance.playerHealth = 0;
                // Llamar al método de muerte
                GameManager.Instance.OnPlayerDied();

                Debug.Log("Player tocó el collider: vida = 0");
            }
        }
    }
}
