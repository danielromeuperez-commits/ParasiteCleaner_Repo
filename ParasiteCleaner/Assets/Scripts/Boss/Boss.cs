using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("HEALTH")]
    public int healthPoints = 3; // Vida inicial

    [Header("Referencias al morir")]
    public Canvas canvasToDisable;     // Canvas a apagar al morir
    public GameObject objectToActivate; // Objeto que aparecerá

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
        // Apagar Canvas
        if (canvasToDisable != null)
        {
            canvasToDisable.gameObject.SetActive(false);
        }

        // Activar objeto y moverlo 3 unidades en Y
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Vector3 newPosition = objectToActivate.transform.position;
            newPosition.y += 3f; // Mueve 3 unidades hacia arriba
            objectToActivate.transform.position = newPosition;
        }

        // Desactiva al boss
        gameObject.SetActive(false);
    }
}
