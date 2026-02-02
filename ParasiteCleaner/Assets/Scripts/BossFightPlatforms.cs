using UnityEngine;
using System.Collections;

public class TriggerBlinkAndSpawnMove : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer spriteToBlink;   // Sprite que parpadea
    public GameObject objectToSpawn;       // Prefab a instanciar
    public Transform spawnPoint;           // Posición inicial del spawn

    [Header("Config")]
    public float timeInsideTrigger = 5f;   // Tiempo dentro del trigger para activar
    public float blinkDuration = 1f;       // Duración total del parpadeo
    public float blinkInterval = 0.2f;     // Intervalo entre cambios de color
    public Color blinkColor = Color.red;   // Color del parpadeo
    public float moveDistance = 5f;        // Unidades que se moverá hacia arriba
    public float moveSpeed = 2f;           // Velocidad del objeto instanciado
    public float lifetime = 3f;            // Tiempo antes de despawnear el objeto

    private float timer = 0f;
    private bool isInside = false;
    private bool actionTriggered = false;

    void Update()
    {
        if (isInside && !actionTriggered)
        {
            timer += Time.deltaTime;
            if (timer >= timeInsideTrigger)
            {
                actionTriggered = true;
                StartCoroutine(BlinkAndSpawn());
            }
        }
    }

    // ===================== DETECCIÓN DEL TRIGGER =====================
    void OnTriggerEnter2D(Collider2D other)
    {
        isInside = true;
        timer = 0f;
        actionTriggered = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isInside = false;
        timer = 0f;
        actionTriggered = false;
    }

    // ===================== PARPADEO Y SPAWN =====================
    IEnumerator BlinkAndSpawn()
    {
        // Parpadeo con color
        if (spriteToBlink != null)
        {
            Color originalColor = spriteToBlink.color;
            float elapsed = 0f;
            bool toggle = false;

            while (elapsed < blinkDuration)
            {
                spriteToBlink.color = toggle ? blinkColor : originalColor;
                toggle = !toggle;
                yield return new WaitForSeconds(blinkInterval);
                elapsed += blinkInterval;
            }

            spriteToBlink.color = originalColor; // Restaurar color original
        }

        // Instanciar objeto y mover hacia arriba
        if (objectToSpawn != null && spawnPoint != null)
        {
            GameObject obj = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);
            StartCoroutine(MoveUpAndDespawn(obj));
        }
    }

    IEnumerator MoveUpAndDespawn(GameObject obj)
    {
        if (obj == null) yield break;

        Vector3 startPos = obj.transform.position;
        Vector3 targetPos = startPos + Vector3.up * moveDistance;
        float elapsed = 0f;

        while (obj != null && Vector3.Distance(obj.transform.position, targetPos) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPos, moveSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;

            if (elapsed >= lifetime)
            {
                Destroy(obj);
                yield break;
            }

            yield return null;
        }

        if (obj != null)
        {
            yield return new WaitForSeconds(Mathf.Max(0f, lifetime - elapsed));
            Destroy(obj);
        }
    }
}
