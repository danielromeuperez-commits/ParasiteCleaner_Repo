using UnityEngine;
using System.Collections;

public class MovingPlatform2D : MonoBehaviour
{
    [Header("Waypoints & Movement Configuration")]
    [SerializeField] float speed; //Velocidad de la plataforma
    [SerializeField] Transform[] points; //Array de puntos a perseguir por la plataforma (mínimo 2)
    [SerializeField] int startingPoint; //Define la posición inicial de la plataforma

    int i; //Indice numérico = número de punto a perseguir (pto actual +1 , al llegar al final 0)

    void Start()
    {
        i = startingPoint; //Definir el primer punto a seguir
        transform.position = points[startingPoint].position; //Setear la posición inicial
        StartCoroutine(PlatformMovement()); //Iniciar movimiento
    }

    IEnumerator PlatformMovement()
    {
        while (true)
        {
            // Mover la plataforma hacia el waypoint actual
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

            // Si llegó al waypoint
            if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
            {
                i++;
                if (i == points.Length) i = 0;

                // Esperar 1 segundo en el waypoint
                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

}
