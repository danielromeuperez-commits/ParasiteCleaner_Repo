using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BigShot : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject chargedProjectile;
    public Transform shootPoint;

    [Header("Charge Config")]
    public float chargeTime = 3f;
    public Color chargeColor = Color.red;
    public float flashDuration = 0.5f;
    public SpriteRenderer spriteRenderer;

    [Header("Player")]
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

        // Inicia carga
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

        // Soltar tecla
        if (isCharging && kb.spaceKey.wasReleasedThisFrame)
        {
            isCharging = false;

            if (chargedReady && chargedProjectile != null && shootPoint != null && player != null)
            {
                // Instanciar proyectil
                GameObject proj = Instantiate(chargedProjectile, shootPoint.position, Quaternion.identity);
                Projectile projScript = proj.GetComponent<Projectile>();

                projScript.isFacingRight = player.isFacingRight;
                projScript.damage = 20; // daño del proyectil cargado

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