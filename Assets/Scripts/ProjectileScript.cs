using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 10f;
    public float lifetime = 5f; // Destroy after 5 seconds if it doesn't hit anything

    [HideInInspector]
    public float damage; // Set by the turret that fires this projectile
    
    private Vector3 direction;
    private Transform target;
    
    void Start()
    {
        // Destroy projectile after lifetime expires
        Destroy(gameObject, lifetime);
    }
    
    void Update()
    {
        // Move projectile forward
        transform.position += direction * speed * Time.deltaTime;
    }
    
    // Set the direction for the projectile to travel
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        
        // Rotate projectile to face direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    // Alternative: Make projectile track a moving target
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if we hit a crow
        if (collision.CompareTag("Crow"))
        {
            Debug.Log("Projectile hit a crow!");

            // Deal damage to the crow
            CrowScript crow = collision.GetComponent<CrowScript>();
            if (crow != null)
            {
                crow.TakeDamage(damage);
            }
            Debug.Log("Dealt " + damage + " damage to crow.");
            Debug.Log("Crow health after hit: " + (crow != null ? crow.health.ToString() : "N/A"));

            // Destroy the projectile
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we hit a crow (if using collision instead of trigger)
        if (collision.gameObject.CompareTag("Crow"))
        {
            Debug.Log("Projectile hit a crow!");

            // Deal damage to the crow
            CrowScript crow = collision.gameObject.GetComponent<CrowScript>();
            if (crow != null)
            {
                crow.TakeDamage(damage);
            }
            Debug.Log("Dealt " + damage + " damage to crow.");
            Debug.Log("Crow health after hit: " + (crow != null ? crow.health.ToString() : "N/A"));

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}