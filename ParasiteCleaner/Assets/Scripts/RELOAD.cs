using UnityEngine;
using UnityEngine.SceneManagement;

public class RELOAD : MonoBehaviour
{
    // ================== RECARGAR ESCENA ACTUAL ==================
    public void ReloadScene()
    {
        if (GameManager.Instance != null)
        {

            GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;


            // Como en tu GameManager es private, usamos reflection para mantenerlo seguro
            var gameOverField = typeof(GameManager).GetField("gameOverTriggered",
                                       System.Reflection.BindingFlags.NonPublic |
                                       System.Reflection.BindingFlags.Instance);
            if (gameOverField != null)
                gameOverField.SetValue(GameManager.Instance, false);

            // Asegurarse de que Time.timeScale esté activo
            Time.timeScale = 1f;
        }

        // Recargar la escena actual
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // ================== CARGAR ESCENA POR NOMBRE ==================

    public void LoadScene(string sceneName)
    {
        if (GameManager.Instance != null)
        {

            GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;

            var gameOverField = typeof(GameManager).GetField("gameOverTriggered",
                                       System.Reflection.BindingFlags.NonPublic |
                                       System.Reflection.BindingFlags.Instance);
            if (gameOverField != null)
                gameOverField.SetValue(GameManager.Instance, false);

            Time.timeScale = 1f;
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No se ha asignado ningún nombre de escena para LoadScene.");
        }
    }
}
