using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("HEALTH")]
    public int healthPoints = 3; // Vida inicial

    [Header("Referencias al morir")]
    public Canvas canvasToDisable;      // Canvas a apagar al morir
    public GameObject objectToActivate; // Objeto que aparecerá
    public float moveUpDistance = 3f;   // Distancia a mover hacia arriba
    public float moveSpeed = 2f;        // Velocidad ajustable del movimiento

    // Variables internas
    private bool isMoving = false;
    private Vector3 targetPosition;

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

        // Activar objeto y preparar movimiento
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            targetPosition = objectToActivate.transform.position + new Vector3(0f, moveUpDistance, 0f);
            isMoving = true;
        }

        // Desactiva al boss
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // Mover el objeto suavemente hacia arriba
        if (isMoving && objectToActivate != null)
        {
            objectToActivate.transform.position = Vector3.MoveTowards(
                objectToActivate.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(objectToActivate.transform.position, targetPosition) < 0.01f)
            {
                // Movimiento completado
                isMoving = false;
            }
        }
    }
}
