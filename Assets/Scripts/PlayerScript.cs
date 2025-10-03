using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections; // Required for Coroutines

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    // --- EXISTING VARIABLES ---
    private Rigidbody2D _rbody;
    public float speed = 5f;
    public int gold = 0; // The document states starting gold is 20, ensure this is set in the Inspector.
    private Vector2 _moveDirection;
    private TurretPlacementManager _placementManager;
    public TMP_Text goldText;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private float _flashDuration = 0.2f;
    private float _flashTimer = 0f;
    private Collider2D _collider;

    // --- NEW HEALTH & RESPAWN VARIABLES ---
    [Header("Health and Respawn")]
    public int maxHealth = 100;
    public float respawnTime = 5.0f;
    public Vector3 respawnPoint = new Vector3(0, -5, 0); // A safe default respawn point

    private int _currentHealth;
    public bool _isDead = false;

    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>(); // Get the collider component
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer != null)
        {
            _originalColor = _spriteRenderer.color;
        }

        // --- NEW ---
        _currentHealth = maxHealth; // Initialize health

        // Find the placement manager in the scene
        _placementManager = FindFirstObjectByType<TurretPlacementManager>();
        if (_placementManager == null)
        {
            Debug.LogWarning("TurretPlacementManager not found in scene!");
        }
    }

    void Update()
    {
        // Update the gold display
        if (goldText != null)
        {
            goldText.text = "Gold: " + gold.ToString();
        }

        // Handle flash timer
        if (_flashTimer > 0)
        {
            _flashTimer -= Time.deltaTime;
            if (_flashTimer <= 0 && _spriteRenderer != null && !_isDead)
            {
                _spriteRenderer.color = _originalColor;
            }
        }
    }

    private void FixedUpdate()
    {
        // --- UPDATED ---
        // Player cannot move if dead OR placing a turret
        if (_isDead || (_placementManager != null && _placementManager.IsPlacing()))
        {
            _rbody.linearVelocity = Vector2.zero; // Use velocity to ensure immediate stop
            return;
        }

        _rbody.linearVelocity = _moveDirection * speed;
    }

    void OnMove(InputValue value)
    {
        // --- UPDATED ---
        // Do not process movement input if dead
        if (_isDead)
        {
            _moveDirection = Vector2.zero;
            return;
        }
        _moveDirection = value.Get<Vector2>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // --- UPDATED ---
        // Cannot pick up coins while dead
        if (_isDead) return;

        if (collider.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Picked up a coin!");
            gold += 1;
            Destroy(collider.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // --- UPDATED ---
        // Cannot take damage while dead
        if (_isDead) return;

        if (collision.gameObject.CompareTag("Crow") || collision.gameObject.CompareTag("Enemy"))
        {
            // You can refine the damage value later
            TakeDamage(10);
        }
    }

    // --- NEW PUBLIC METHOD FOR TAKING DAMAGE ---
    public void TakeDamage(int damage)
    {
        if (_isDead) return; // Cannot take damage if already dead

        _currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Health is now {_currentHealth}/{maxHealth}");
        FlashRed();

        if (_currentHealth <= 0)
        {
            StartCoroutine(DieAndRespawn());
        }
    }

    // --- NEW COROUTINE FOR DEATH AND RESPAWN LOGIC ---
    private IEnumerator DieAndRespawn()
    {
        Debug.Log("Player has died. Respawning in " + respawnTime + " seconds.");
        _isDead = true;

        // Disable player interaction and visuals
        _collider.enabled = false;
        _spriteRenderer.enabled = false;
        _rbody.linearVelocity = Vector2.zero; // Immediately stop movement

        // Wait for the respawn timer
        yield return new WaitForSeconds(respawnTime);

        // Respawn the player
        Debug.Log("Respawning player.");
        transform.position = respawnPoint;
        _currentHealth = maxHealth;

        // Re-enable components
        _spriteRenderer.enabled = true;
        _spriteRenderer.color = _originalColor; // Reset color on respawn
        _collider.enabled = true;
        
        _isDead = false;
    }

    void FlashRed()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = Color.red;
            _flashTimer = _flashDuration;
        }
    }
}