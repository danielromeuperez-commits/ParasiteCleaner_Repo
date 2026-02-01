using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image bosshealthFill;

    [Header("Smooth Fill")]
    [SerializeField] private float smoothSpeeds = 5f;

    [Header("Boss Reference")]
    [SerializeField] private Boss boss; // referencia directa al Boss

    [Header("Boss Max Health")]
    [SerializeField] private int bossMaxHealth = 3; // AJÚSTALO AL MISMO VALOR DEL BOSS

    void Update()
    {
        if (boss == null || bosshealthFill == null) return;

        float targetFill = (float)boss.healthPoints / (float)bossMaxHealth;

        bosshealthFill.fillAmount = Mathf.Lerp(
            bosshealthFill.fillAmount,
            targetFill,
            Time.deltaTime * smoothSpeeds
        );
    }
}
