using UnityEngine;

public class Check : MonoBehaviour
{
    public GameObject canvas;

    private void Start()
    {
        if (canvas != null)
            canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canvas != null)
                canvas.SetActive(true);

            Destroy(gameObject); // opcional
        }
    }
}
