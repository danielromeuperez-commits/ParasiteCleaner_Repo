using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [Header("Scene to Load")]
    public string sceneName; // Nombre de la escena que quieres cargar

    public void LoadScene()
    {
        if (GameManager.Instance != null)
        {
            // Restaurar la vida del jugador sumando la vida máxima
            GameManager.Instance.playerHealth += GameManager.Instance.maxHealth;

            // Evitar que pase del máximo
            if (GameManager.Instance.playerHealth > GameManager.Instance.maxHealth)
            {
                GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
            }
        }

        // Cargar la escena deseada
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No se ha asignado ningún nombre de escena en LoadSceneWithHealthReset.");
        }
    }
}
