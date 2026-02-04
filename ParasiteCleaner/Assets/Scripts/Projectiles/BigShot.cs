using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BigShot : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject chargedProjectile;
    public Transform shootPoint;
    public float chargeTime = 3f;

    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public Color chargingColor = Color.yellow;
    public Color chargedColor = Color.red;
    public float blinkSpeed = 6f;

    [Header("Refs")]
    public PlayerController2D player;

    bool isCharging;
    bool chargedReady;
    float chargeCounter;
    Coroutine blinkRoutine;

    void Update()
    {
        // ================= BLOQUEO SI NO ESTÁ EN EL SUELO =================
        if (!player.IsGrounded)
        {
            if (isCharging) ReleaseCharge(); // cancelar carga si está en el aire
            return; // no permitir iniciar carga
        }

        // ================= INPUT =================
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            StartCharge();

        if (isCharging && Keyboard.current.spaceKey.isPressed)
        {
            chargeCounter += Time.deltaTime;

            // Cuando termina de cargar
            if (!chargedReady && chargeCounter >= chargeTime)
                FullyCharged();
        }

        if (isCharging && Keyboard.current.spaceKey.wasReleasedThisFrame)
            ReleaseCharge();
    }

    // ================= CHARGE =================

    void StartCharge()
    {
        if (!player.IsGrounded) return; // seguridad extra

        isCharging = true;
        chargeCounter = 0f;
        chargedReady = false;

        // Bloquear movimiento y animación del jugador
        player.enabled = false;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        player.GetComponent<Animator>().SetBool("IsBigShotCharging", true);

        // Feedback visual (parpadeo)
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkColor());
    }

    void FullyCharged()
    {
        chargedReady = true;

        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        spriteRenderer.color = chargedColor; // color sólido al estar full
    }

    void ReleaseCharge()
    {
        isCharging = false;

        // Desbloquear jugador
        player.enabled = true;
        player.GetComponent<Animator>().SetBool("IsBigShotCharging", false);

        // Reset visual
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);
        spriteRenderer.color = Color.white;

        // Solo disparar si está en el suelo
        if (chargedReady && player.IsGrounded)
        {
            GameObject proj = Instantiate(chargedProjectile, shootPoint.position, Quaternion.identity);
            AudioManager.Instance.PlaySFX(0);
            proj.GetComponent<Projectile>().isFacingRight = player.IsFacingRight;
            player.GetComponent<Animator>().SetTrigger("BigShot");
        }

        ResetCharge();
    }

    void ResetCharge()
    {
        chargedReady = false;
        chargeCounter = 0f;
        isCharging = false;
    }

    // ================= VISUAL =================

    IEnumerator BlinkColor()
    {
        while (true)
        {
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            spriteRenderer.color = Color.Lerp(Color.white, chargingColor, t);
            yield return null;
        }
    }
}
