using UnityEngine;

public class AOETurret : TurretScript
{
    [Header("AOE Settings")]
    public float explosionRadius = 2f;
    public GameObject aoeProjectilePrefab;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Shoot()
    {
        if (aoeProjectilePrefab == null)
        {
            Debug.LogWarning("AOE Projectile prefab not assigned!");
            return;
        }

        if (currentTarget == null) return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        GameObject projectile = Instantiate(aoeProjectilePrefab, spawnPosition, Quaternion.identity);
        Vector3 direction = (currentTarget.position - spawnPosition).normalized;

        AOEProjectile projScript = projectile.GetComponent<AOEProjectile>();
        if (projScript != null)
        {
            projScript.SetDirection(direction);
            projScript.damage = damage;
            projScript.explosionRadius = explosionRadius;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (showRangeGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}