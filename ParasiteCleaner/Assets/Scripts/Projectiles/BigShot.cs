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
        if (!player.IsGrounded)
        {
            if (isCharging) ReleaseCharge();
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            StartCharge();

        if (isCharging && Keyboard.current.spaceKey.isPressed)
        {
            chargeCounter += Time.deltaTime;

            if (!chargedReady && chargeCounter >= chargeTime)
                FullyCharged();
        }

        if (isCharging && Keyboard.current.spaceKey.wasReleasedThisFrame)
            ReleaseCharge();
    }

    void StartCharge()
    {
        if (!player.IsGrounded) return;

        isCharging = true;
        chargeCounter = 0f;
        chargedReady = false;
        player.enabled = false;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        player.GetComponent<Animator>().SetBool("IsBigShotCharging", true);
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkColor());
    }

    void FullyCharged()
    {
        chargedReady = true;

        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        spriteRenderer.color = chargedColor;
    }

    void ReleaseCharge()
    {
        isCharging = false;

        player.enabled = true;
        player.GetComponent<Animator>().SetBool("IsBigShotCharging", false);

        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);
        spriteRenderer.color = Color.white;

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
