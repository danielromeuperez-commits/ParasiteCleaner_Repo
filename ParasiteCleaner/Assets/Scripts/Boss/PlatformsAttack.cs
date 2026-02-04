using UnityEngine;
using System.Collections;

public class PlatformsAttack : MonoBehaviour
{
    [Header("Timers")]
    [SerializeField] float blinkDelay = 3f;
    [SerializeField] float spawnDelay = 5f;

    [Header("Blink")]
    [SerializeField] Color blinkColor = Color.red;
    [SerializeField] float blinkSpeed = 0.15f;

    [Header("Spawn")]
    [SerializeField] Transform spawnPoint;   // GameObject vacío
    [SerializeField] GameObject risingPrefab;

    SpriteRenderer sr;
    Color originalColor;

    Coroutine mainCoroutine;
    Coroutine blinkCoroutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        if (mainCoroutine == null)
            mainCoroutine = StartCoroutine(PlatformSequence(col.transform));
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        StopAllCoroutines();
        mainCoroutine = null;
        blinkCoroutine = null;
        sr.color = originalColor;
    }

    IEnumerator PlatformSequence(Transform player)
    {
        // Espera hasta parpadeo
        yield return new WaitForSeconds(blinkDelay);

        blinkCoroutine = StartCoroutine(BlinkRed());

        // Espera hasta spawn
        yield return new WaitForSeconds(spawnDelay - blinkDelay);

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        sr.color = originalColor;

        SpawnObject(player);
    }

    IEnumerator BlinkRed()
    {
        while (true)
        {
            sr.color = blinkColor;
            yield return new WaitForSeconds(blinkSpeed);
            sr.color = originalColor;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    void SpawnObject(Transform player)
    {
        Vector3 spawnPos = new Vector3(
            player.position.x,      // X del player
            spawnPoint.position.y,  // Y del GameObject vacío
            0f
        );

        Instantiate(risingPrefab, spawnPos, Quaternion.identity);
    }
}
