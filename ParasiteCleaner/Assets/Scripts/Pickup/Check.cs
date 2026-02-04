using UnityEngine;

public class Check : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasToEnable;
    public GameObject canvasToDisable;

    [Header("Audio")]
    public GameObject music;
    public GameObject ambient;
    public int firstSFX = 0;
    public int secondSFX = 1;

    private bool triggered = false;

    private void Start()
    {
        if (canvasToEnable != null)
            canvasToEnable.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered)
        {
            triggered = true;
            if (canvasToEnable != null)
                canvasToEnable.SetActive(true);

            if (canvasToDisable != null)
                canvasToDisable.SetActive(false);  
            Time.timeScale = 0f;
            if (music != null) music.SetActive(false);
            if (ambient != null) ambient.SetActive(false);

            // Reproducir sonidos
            AudioManager.Instance.PlaySFX(firstSFX);
            AudioManager.Instance.PlaySFX(secondSFX);
        }
    }
}
