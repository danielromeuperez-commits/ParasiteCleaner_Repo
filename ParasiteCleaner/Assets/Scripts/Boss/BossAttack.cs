using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    [Header("Attack Config")]
    public GameObject shootPoint;       // GameObject vacío desde donde se dispara
    public Transform playerTransform;    // Referencia al jugador
    public float attackSpeed = 10f;      // Velocidad del proyectil
    public float duration = 2f;          // Tiempo que dura activo el proyectil
    public GameObject attackPrefab;      // Prefab del proyectil

    [Header("Attack Interval")]
    public float attackInterval = 1f;    // Tiempo entre cada ataque
    private bool attacking = false;

    private void Start()
    {
        if (shootPoint != null && playerTransform != null && attackPrefab != null)
        {
            attacking = true;
            StartCoroutine(AutoAttack());
        }
    }

    // Corrutina que dispara automáticamente cada 'attackInterval'
    IEnumerator AutoAttack()
    {
        while (attacking)
        {
            Attack1();
            yield return new WaitForSeconds(attackInterval);
        }
    }

    // ===================== ATAQUE =====================
    void Attack1()
    {
        // Spawn del proyectil alineado con Y del jugador
        Vector3 spawnPos = new Vector3(
            shootPoint.transform.position.x,
            playerTransform.position.y,
            shootPoint.transform.position.z
        );

        GameObject proj = Instantiate(attackPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.left * attackSpeed;
        }

        StartCoroutine(DesactivarDespues(proj, duration));
    }

    // ===================== UTILIDADES =====================
    IEnumerator DesactivarDespues(GameObject obj, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (obj != null)
            obj.SetActive(false);
    }
}
