using UnityEngine;

public class VoidFalling : MonoBehaviour
{
    public Transform teleport;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleport.position;
            GameManager.Instance.playerHealth -= 1;
        }
    }
}
