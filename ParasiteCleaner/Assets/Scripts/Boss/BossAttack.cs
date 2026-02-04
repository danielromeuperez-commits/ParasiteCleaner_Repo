using UnityEngine;
using System.Collections;

public class BossAttackAction : MonoBehaviour
{
    [Header("References")]
    public GameObject shootPoint;
    public Transform playerTransform;
    public GameObject attackPrefab;

    [Header("Attack Config")]
    public float attackSpeed = 10f;
    public float projectileDuration = 2f;
    public void Shoot()
    {
        if (shootPoint == null || playerTransform == null || attackPrefab == null)
            return;

        Vector3 spawnPos = new Vector3(
            shootPoint.transform.position.x,
            playerTransform.position.y,
            shootPoint.transform.position.z
        );

        GameObject proj = Instantiate(attackPrefab, spawnPos, Quaternion.identity);
        AudioManager.Instance.PlaySFX(11);

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.left * attackSpeed; 
        }

        StartCoroutine(DisableAfterTime(proj, projectileDuration));
    }
    IEnumerator DisableAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj != null)
            obj.SetActive(false);
    }
}
