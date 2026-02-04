using UnityEngine;

public class Death : MonoBehaviour
{
    [Header("Settings")]
    public string playerTag = "Player";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (GameManager.Instance != null)
            {

                GameManager.Instance.playerHealth = 0;

                GameManager.Instance.OnPlayerDied();

                Debug.Log("Player tocó el collider: vida = 0");
            }
        }
    }
}
