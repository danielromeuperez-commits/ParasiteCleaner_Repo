using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Configuración del Pickup")]
    public int jumpForceIncrease = 5;        // Cantidad de jumpForce a añadir
    public Animator animator;                // Animator del pickup
    public string pickupBoolName = "Pick"; // Nombre del bool en el Animator

    private bool pickedUp = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica que sea el Player
        PlayerController2D player = collision.GetComponent<PlayerController2D>();
        if (player != null && !pickedUp)
        {
            pickedUp = true;

            // Activa la animación cambiando el bool
            if (animator != null)
            {
                animator.SetBool(pickupBoolName, true);
            }

            // Aumenta el jumpForce del player
            player.jumpForce += jumpForceIncrease;

            // Inicia coroutine para esperar a que la animación termine
            if (animator != null)
            {
                StartCoroutine(WaitForAnimationToEnd());
            }
            else
            {
                Destroy(gameObject); // Si no hay animator, destruye inmediatamente
            }
        }
    }

    private System.Collections.IEnumerator WaitForAnimationToEnd()
    {
        // Espera hasta que la animación activa termine
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // Si la animación puede ser más larga por transición, espera un poquito más
        yield return new WaitForSeconds(animationLength);

        // Desactiva o destruye el pickup
        gameObject.SetActive(false);
    }
}
