using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) Debug.Log("No hay GameManager!");
            return instance;
        }
    }

    public float playerHealth;
    public float maxHealth = 100;
    public int playerPoints;
    public bool IsPlayerDead => playerHealth <= 0;

    // REFERENCIAS PARA EL GAME OVER
    public Image fadeImage;            // Imagen negra para el fade
    public Canvas gameOverCanvas;      // Canvas completo que incluye texto y botones
    public float fadeDuration = 1f;    // Duración del fade
    public float delayBeforeGameOver = 2f; // Segundos antes de iniciar TODO

    private bool gameOverTriggered = false;

    private void Awake()
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

    private void Start()
    {
        // Al inicio, ocultamos canvas y fade
        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(false);
        if (fadeImage != null)
            fadeImage.gameObject.SetActive(false); // oculto al inicio
    }

    private void Update()
    {
        if (playerHealth < 0) playerHealth = 0;

        if (!gameOverTriggered && IsPlayerDead)
        {
            gameOverTriggered = true;
            StartCoroutine(HandleGameOver());
        }
    }

    private IEnumerator HandleGameOver()
    {
        // Esperamos el delay antes de iniciar TODO
        yield return new WaitForSecondsRealtime(delayBeforeGameOver);

        // Activamos la imagen de fade
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0); // transparente al inicio

        // Fade-in a negro usando unscaledDeltaTime
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // independiente de Time.timeScale
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = Color.black;

        // Ahora pausamos el juego
        Time.timeScale = 0f;

        // Activamos el canvas completo con texto/botones
        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(true);
    }

}
