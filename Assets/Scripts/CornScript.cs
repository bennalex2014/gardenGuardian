using UnityEngine;

public class CornScript : MonoBehaviour
{

    public int health = 100;
    public int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Crow"))
        {
            CrowScript crow = other.GetComponent<CrowScript>();
            if (crow != null)
            {
                currentHealth -= crow.damageToCorn;
                Destroy(other.gameObject); // Destroy the crow on collision
            }
        }
    }   
}
