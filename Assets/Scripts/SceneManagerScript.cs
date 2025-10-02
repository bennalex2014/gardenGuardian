using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [Header("References")]
    public CornScript corn;
    
    [Header("Settings")]
    public bool checkCornHealth = true;
    
    private bool gameEnded = false;
    
    void Start()
    {
        // Try to find corn if not assigned
        if (corn == null)
        {
            corn = FindFirstObjectByType<CornScript>();
            
            if (corn == null)
            {
                Debug.LogError("CornScript not found! Assign it in the Inspector or add it to the scene.");
            }
        }
    }
    
    void Update()
    {
        if (!gameEnded && checkCornHealth && corn != null)
        {
            // Check if corn is dead
            if (corn.health <= 0)
            {
                EndGame();
            }
        }
    }
    
    void EndGame()
    {
        gameEnded = true;
        
        Debug.Log("Game Over! Corn health reached 0.");
        
        // For now, just stop the game
        // Later you can load an end screen scene
        Time.timeScale = 0f; // Pauses the game
        
        // TODO: Show end screen UI
        // TODO: Load end scene when you create it
        // SceneManager.LoadScene("EndScreen");
    }
    
    // Public method to restart the game (call this from a UI button later)
    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Public method to load a specific scene (for future use)
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // Unpause
        SceneManager.LoadScene(sceneName);
    }
}