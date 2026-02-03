using UnityEngine;

public class Check : MonoBehaviour
{
    public GameObject canvas;
    public GameObject music;
    public GameObject ambient;

    private void Start()
    {
        if (canvas != null)
            canvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canvas != null)
                canvas.SetActive(true);
            Time.timeScale = 0f;
            music.SetActive(false);
            ambient.SetActive(false);

        }
    }
}
