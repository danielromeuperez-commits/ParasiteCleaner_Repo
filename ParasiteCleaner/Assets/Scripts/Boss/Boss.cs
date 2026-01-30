using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("HEALTH")]
    public int healthPoints = 3; // Vida inicial

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            Die();
        }
    }

    // Método para morir
    void Die()
    {
        gameObject.SetActive(false);
    }
}
