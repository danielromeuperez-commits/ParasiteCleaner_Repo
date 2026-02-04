using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("HEALTH")]
    public int healthPoints = 3;

    [Header("Referencias al morir")]
    public Canvas canvasToDisable;
    public GameObject objectToActivate;
    public float moveUpDistance = 3f;
    public float moveSpeed = 2f;

    [Header("Trampas a desactivar")]
    public Collider2D[] trapColliders;

    private bool isMoving = false;
    private Vector3 targetPosition;

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (canvasToDisable != null)
        {
            canvasToDisable.gameObject.SetActive(false);
        }

        if (trapColliders != null && trapColliders.Length > 0)
        {
            foreach (Collider2D col in trapColliders)
            {
                if (col != null)
                    col.enabled = false;
            }
        }
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            targetPosition = objectToActivate.transform.position + new Vector3(0f, moveUpDistance, 0f);
            isMoving = true;
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isMoving && objectToActivate != null)
        {
            objectToActivate.transform.position = Vector3.MoveTowards(
                objectToActivate.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(objectToActivate.transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }
}
