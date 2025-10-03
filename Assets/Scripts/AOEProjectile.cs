using UnityEngine;

public class AOEProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 8f;
    public float damage = 20f;
    public float lifetime = 5f;
    public float explosionRadius = 2f;

    [Header("Visual Effects")]
    public GameObject explosionEffectPrefab; // Optional explosion visual

    private Vector2 direction;
    private bool hasExploded = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Explode on hitting a crow
        if (collision.CompareTag("Crow") && !hasExploded)
        {
            Explode(collision.transform.position);
        }
    }

    void Explode(Vector3 explosionCenter)
    {
        hasExploded = true;

        // Find all crows in explosion radius
        Collider2D[] hitCrows = Physics2D.OverlapCircleAll(explosionCenter, explosionRadius);

        foreach (Collider2D col in hitCrows)
        {
            if (col.CompareTag("Crow"))
            {
                CrowScript crow = col.GetComponent<CrowScript>();
                if (crow != null)
                {
                    crow.TakeDamage(damage);
                }
            }
        }

        // Spawn explosion visual effect
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, explosionCenter, Quaternion.identity);
            Destroy(explosion, 1f); // Destroy effect after 1 second
        }

        Debug.Log($"AOE Explosion at {explosionCenter}! Radius: {explosionRadius}");

        // Destroy projectile
        Destroy(gameObject);
    }
}