using UnityEngine;
using UnityEngine.UI;
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
    public Image fadeImage;
    public Canvas gameOverCanvas;
    public float fadeDuration = 1f;
    public float delayBeforeGameOver = 2f;

    bool gameOverTriggered;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerHealth = maxHealth;

        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(false);

        if (fadeImage != null)
            fadeImage.gameObject.SetActive(false);
    }

    // ================= DAMAGE =================
    public void TakeDamage(float dmg)
    {
        if (IsPlayerDead) return;

        playerHealth -= dmg;
        if (playerHealth < 0) playerHealth = 0;
    }

    // ================= PLAYER DIED =================
    public void OnPlayerDied()
    {
        if (gameOverTriggered) return;

        gameOverTriggered = true;
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        // Esperar a que termine la animación de muerte
        yield return new WaitForSeconds(delayBeforeGameOver);

        // Fade a negro
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float a = elapsed / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = Color.black;

        // Pausar TODO
        Time.timeScale = 0f;

        // Mostrar Game Over
        gameOverCanvas.gameObject.SetActive(true);
    }
}
