using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [Header("Turret Stats")]
    public float damage = 50f;
    public float fireRate = 1f; // Shots per second
    public float range = 5f;
    public int goldCost = 10;
    public int health = 100;
    public int maxHealth = 100;

    [Header("References")]
    public GameObject projectilePrefab;
    public Transform firePoint; // Where projectiles spawn from

    [Header("Debug")]
    public bool showRangeGizmo = true;

    protected float fireTimer = 0f;
    protected Transform currentTarget;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private float _flashDuration = 0.2f;
    private float _flashTimer = 0f;

    protected virtual void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            _originalColor = _spriteRenderer.color;
        }
    }

    protected virtual void Update()
    {
        // Find target if we don't have one
        if (currentTarget == null)
        {
            FindTarget();
        }
        else
        {
            // Check if target is still in range
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget > range)
            {
                currentTarget = null; // Target out of range, find a new one
            }
        }

        // Shoot at target if we have one
        if (currentTarget != null)
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= 1f / fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }

        // Flash effect
        if (_flashTimer > 0)
        {
            _flashTimer -= Time.deltaTime;
            if (_flashTimer <= 0 && _spriteRenderer != null)
            {
                _spriteRenderer.color = _originalColor;
            }
        }
    }

    protected virtual void FindTarget()
    {
        // Find all crows in the scene
        GameObject[] crows = GameObject.FindGameObjectsWithTag("Crow");

        float closestDistance = range;
        Transform closestCrow = null;

        // Find the closest crow within range
        foreach (GameObject crow in crows)
        {
            float distance = Vector3.Distance(transform.position, crow.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCrow = crow.transform;
            }
        }

        currentTarget = closestCrow;
    }

    protected virtual void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab not assigned to turret!");
            return;
        }

        if (currentTarget == null) return;

        // Determine spawn position
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;

        // Create projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Calculate direction to target
        Vector3 direction = (currentTarget.position - spawnPosition).normalized;

        // Set projectile direction and damage
        ProjectileScript projScript = projectile.GetComponent<ProjectileScript>();
        if (projScript != null)
        {
            projScript.SetDirection(direction);
            projScript.damage = damage;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Turret took " + damage + " damage. Health: " + health);

        if (health <= 0)
        {
            DestroyTurret();
        }
    }

    protected virtual void DestroyTurret()
    {
        Debug.Log("Turret destroyed!");
        Destroy(gameObject);
    }

    // Visualize range in editor
    void OnDrawGizmosSelected()
    {
        if (showRangeGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }

    public void FlashRed()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = Color.red;
            _flashTimer = _flashDuration;
        }
    }
}