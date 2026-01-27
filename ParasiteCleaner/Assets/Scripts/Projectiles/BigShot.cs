using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BigShot : MonoBehaviour
{
    [Header("Proyectil cargado")]
    public GameObject chargedProjectile;
    public Transform shootPoint;

    [Header("Carga")]
    public float chargeTime = 5f;
    public Color chargeColor = Color.red;
    public float flashDuration = 0.5f;
    public SpriteRenderer spriteRenderer;

    // Referencia al jugador para saber la dirección
    public PlayerController2D player;

    private bool isCharging = false;
    private bool chargedReady = false;
    private float chargeCounter = 0f;
    private Keyboard kb;

    void Awake()
    {
        kb = Keyboard.current;
    }

    void Update()
    {
        if (kb == null) return;

        // Inicia la carga
        if (kb.spaceKey.wasPressedThisFrame)
        {
            isCharging = true;
            chargeCounter = 0f;
            chargedReady = false;
        }

        // Mantener pulsado
        if (isCharging && kb.spaceKey.isPressed)
        {
            chargeCounter += Time.deltaTime;

            if (!chargedReady && chargeCounter >= chargeTime)
            {
                chargedReady = true;
                if (spriteRenderer != null)
                    StartCoroutine(FlashColor(flashDuration));
            }
        }

        // Soltar la tecla
        if (isCharging && kb.spaceKey.wasReleasedThisFrame)
        {
            isCharging = false;

            // Disparar proyectil cargado si estaba listo
            if (chargedReady && chargedProjectile != null && shootPoint != null && player != null)
            {
                GameObject proj = Instantiate(chargedProjectile, shootPoint.position, Quaternion.identity);
                Projectile projScript = proj.GetComponent<Projectile>();
                projScript.isFacingRight = player.isFacingRight; // <-- clave
            }

            chargedReady = false;
            chargeCounter = 0f;
        }
    }

    private IEnumerator FlashColor(float duration)
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = chargeColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }
}
