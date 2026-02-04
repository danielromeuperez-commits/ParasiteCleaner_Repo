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
    [SerializeField] SpriteRenderer blinkTarget;  // El SpriteRenderer que parpadea

    [Header("Spawn")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject risingPrefab;

    SpriteRenderer spriter;
    Color originalColor;

    Coroutine mainCoroutine;
    Coroutine blinkCoroutine;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        originalColor = blinkTarget != null ? blinkTarget.color : spriter.color;
    }

void OnTriggerEnter2D(Collider2D col)
    {
        if (!CompareTag("BlinkSpawnTrigger")) return;

        if (col.CompareTag("Player") && mainCoroutine == null)
            mainCoroutine = StartCoroutine(PlatformSequence(col.transform));
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!CompareTag("BlinkSpawnTrigger")) return;
        if (!col.CompareTag("Player")) return;

        StopAllCoroutines();
        mainCoroutine = null;
        blinkCoroutine = null;
        blinkTarget.color = originalColor;
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

        if (blinkTarget != null)
            blinkTarget.color = originalColor;
        else
            spriter.color = originalColor;

        SpawnObject(player);
    }

    IEnumerator BlinkRed()
    {
        SpriteRenderer target = blinkTarget != null ? blinkTarget : spriter;

        while (true)
        {
            target.color = blinkColor;
            yield return new WaitForSeconds(blinkSpeed);
            target.color = originalColor;
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
