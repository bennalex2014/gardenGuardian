using UnityEngine;

public class CornScript : MonoBehaviour
{

    public int health = 100;
    public int currentHealth;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private float _flashDuration = 0.2f;
    private float _flashTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = health;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            _originalColor = _spriteRenderer.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        // Handle flash timer
        if (_flashTimer > 0)
        {
            _flashTimer -= Time.deltaTime;
            if (_flashTimer <= 0 && _spriteRenderer != null)
            {
                _spriteRenderer.color = _originalColor;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Crow"))
        {
            CrowScript crow = collision.gameObject.GetComponent<CrowScript>();
            if (crow != null)
            {
                currentHealth -= crow.damageToCorn;
                crow.killCrow(false); // Destroy the crow on collision
                FlashRed();
            }
        }
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
