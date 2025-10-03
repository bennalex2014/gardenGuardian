using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CrowScript : MonoBehaviour
{
    private Rigidbody2D _rbody;
    public float speed = 3f;
    public float detectionRadius = 5f;
    public float rotationSpeed = 5f;
    public Vector2 mapCenter = Vector2.zero;
    private Transform _playerTransform;
    public int damageToPlayer = 20;
    public int damageToCorn = 10;
    public int damageToTurret = 15;
    public float attackCooldown = 1f; // Time between attacks
    public int health = 100;
    public GameObject coinPrefab;
    public int goldOnDeath = 2;

    private WaveManagerScript waveManager;
    private float attackTimer = 0f;

    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }

        // Find wave manager for tracking
        waveManager = FindFirstObjectByType<WaveManagerScript>();
    }

    void Update()
    {
        // Update attack timer
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 targetPosition;

        if (_playerTransform != null && _playerTransform.GetComponent<PlayerScript>()._isDead == false)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

            if (distanceToPlayer <= detectionRadius)
            {
                targetPosition = _playerTransform.position;
            }
            else
            {
                targetPosition = mapCenter;
            }
        }
        else
        {
            targetPosition = mapCenter;
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        _rbody.linearVelocity = direction * speed;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        health -= (int)damage;
        if (health <= 0)
        {
            killCrow();
        }
    }

    public void killCrow(Boolean dropsGold = true)
    {
        // Spawn coins
        if (dropsGold)
        {
            for (int i = 0; i < goldOnDeath; i++)
            {
                Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
                Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
                Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            }
        }

        // Notify wave manager that a crow died
        if (waveManager != null)
        {
            waveManager.OnCrowDied();
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Turret"))
        {
            TurretScript turret = collision.gameObject.GetComponent<TurretScript>();
            if (turret != null)
            {
                turret.TakeDamage(damageToTurret);
                Debug.Log("Crow hit turret for " + damageToTurret + " damage and died");
            }
            collision.gameObject.GetComponent<TurretScript>().FlashRed();
            killCrow(false); // Crow dies on impact with turret
        }
    }
}