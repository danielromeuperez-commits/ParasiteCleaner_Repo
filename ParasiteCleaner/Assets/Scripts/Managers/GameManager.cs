using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [Header("Player")]
    public float maxHealth = 100f;
    public float playerHealth;
    public bool IsPlayerDead => playerHealth <= 0;

    [Header("Game Over")]
    public Image fadeImage; // Opcional, si quieres fade negro
    public Canvas gameOverCanvas; // Será buscado automáticamente si es null
    public float fadeDuration = 1f;
    public float delayBeforeGameOver = 2f;

    private bool gameOverTriggered;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetGameState();
    }

    public void TakeDamage(float dmg)
    {
        if (IsPlayerDead) return;

        playerHealth -= dmg;
        if (playerHealth < 0) playerHealth = 0;

        if (IsPlayerDead)
            OnPlayerDied();
    }

    public void OnPlayerDied()
    {
        if (gameOverTriggered) return;

        gameOverTriggered = true;
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        yield return new WaitForSecondsRealtime(delayBeforeGameOver); // No depende de Time.timeScale

        // Si no se asignó fadeImage, intentar buscarlo automáticamente
        if (fadeImage == null)
        {
            GameObject fadeObj = GameObject.Find("FadeImage");
            if (fadeObj != null)
                fadeImage = fadeObj.GetComponent<Image>();
        }

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0);

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeImage.color = new Color(0, 0, 0, elapsed / fadeDuration);
                yield return null;
            }
        }

        Time.timeScale = 0f;

        // Buscar automáticamente el GameOver Canvas si no está asignado
        if (gameOverCanvas == null)
        {
            GameObject canvasObj = GameObject.Find("GameOver");
            if (canvasObj != null)
                gameOverCanvas = canvasObj.GetComponent<Canvas>();
        }

        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(true);
        else
            Debug.LogWarning("No se encontró un Canvas llamado 'GameOver' en la escena.");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetGameState();
    }

    public void ResetGameState()
    {
        Time.timeScale = 1f;
        playerHealth = maxHealth;
        gameOverTriggered = false;

        // Si no está asignado, buscar el Canvas automáticamente
        if (gameOverCanvas == null)
        {
            GameObject canvasObj = GameObject.Find("GameOver");
            if (canvasObj != null)
                gameOverCanvas = canvasObj.GetComponent<Canvas>();
        }

        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(false);

        if (fadeImage != null)
            fadeImage.gameObject.SetActive(false);
    }

    public void RestartCurrentScene()
    {
        ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadSceneByName(string sceneName)
    {
        ResetGameState();
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
    }
}
