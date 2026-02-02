using UnityEngine;
using System.Collections;

public class BossAttackTrigger : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Config")]
    public string attackTriggerName = "Attack";
    public float attackCooldown = 10f;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        // Comienza la rutina para activar el trigger cada attackCooldown segundos
        StartCoroutine(TriggerAttackRoutine());
    }

    IEnumerator TriggerAttackRoutine()
    {
        while (true)
        {
            // Activa el trigger
            animator.ResetTrigger(attackTriggerName);
            animator.SetTrigger(attackTriggerName);

            yield return new WaitForSeconds(attackCooldown);
        }
    }
}
