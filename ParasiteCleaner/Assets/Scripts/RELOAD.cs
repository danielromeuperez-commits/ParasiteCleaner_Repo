using UnityEngine;
using UnityEngine.SceneManagement;

public class RELOAD : MonoBehaviour
{
    [Header("Botón para recargar")]
    public KeyCode reloadKey = KeyCode.R; // Cambia la tecla si quieres

    void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            // Restaurar vida máxima
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerHealth = GameManager.Instance.playerHealth;
            }

            // Recargar la escena actual
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}
