using UnityEngine;

public class RisingObject : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float riseSpeed = 2f;      // VELOCIDAD AJUSTABLE
    [SerializeField] float riseDistance = 6f;   // CUÁNTO SUBE

    [Header("Lifetime")]
    [SerializeField] float lifeTime = 10f;      // CUÁNTO TIEMPO EXISTE

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // Se destruye después de X segundos
        Destroy(gameObject, lifeTime);
        AudioManager.Instance.PlaySFX(13);
    }

    void Update()
    {
        // Sube en Y+
        if (transform.position.y < startPos.y + riseDistance)
        {
            transform.Translate(Vector2.up * riseSpeed * Time.deltaTime);
        }
    }
}
