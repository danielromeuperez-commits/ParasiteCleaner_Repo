using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Configuración del Pickup")]
    public int jumpForceIncrease = 5;
    public Animator animator;
    public string pickupBoolName = "Pick";

    private bool pickedUp = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController2D player = collision.GetComponent<PlayerController2D>();

        if (player != null && !pickedUp)
        {
            pickedUp = true;

            if (animator != null)
            {
                animator.SetBool(pickupBoolName, true);
            }

            player.jumpForce += jumpForceIncrease;
            AudioManager.Instance.PlaySFX(5);

            if (animator != null)
            {
                StartCoroutine(WaitForAnimationToEnd());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private System.Collections.IEnumerator WaitForAnimationToEnd()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);
        gameObject.SetActive(false);
    }
}
