using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [Header("References")]
    public CornScript corn;
    public WaveManagerScript waveManager;
    
    [Header("Settings")]
    public bool checkCornHealth = true;
    public bool checkWaveCompletion = true;
    
    private bool gameEnded = false;
    private bool playerWon = false;
    
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
        
        // Try to find wave manager if not assigned
        if (waveManager == null)
        {
            waveManager = FindFirstObjectByType<WaveManagerScript>();
            
            if (waveManager == null)
            {
                Debug.LogError("WaveManagerScript not found! Assign it in the Inspector or add it to the scene.");
            }
        }
    }
    
    void Update()
    {
        if (gameEnded)
        {
            // Allow restart with R key
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            return;
        }
        
        // Check lose condition: corn died
        if (checkCornHealth && corn != null && corn.currentHealth <= 0)
        {
            LoseGame();
        }
        
        // Check win condition: all waves complete
        if (checkWaveCompletion && waveManager != null && waveManager.allWavesComplete)
        {
            WinGame();
        }
    }
    
    void OnGUI()
    {
        if (gameEnded)
        {
            // Style the text
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 60;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;
            
            GUIStyle smallStyle = new GUIStyle(GUI.skin.label);
            smallStyle.fontSize = 30;
            smallStyle.alignment = TextAnchor.MiddleCenter;
            smallStyle.normal.textColor = Color.white;
            
            // Display win or lose message
            if (playerWon)
            {
                GUI.Label(new Rect(0, Screen.height / 2 - 100, Screen.width, 100), "VICTORY!", style);
                GUI.Label(new Rect(0, Screen.height / 2 + 20, Screen.width, 60), "All waves defeated!", smallStyle);
            }
            else
            {
                GUI.Label(new Rect(0, Screen.height / 2 - 100, Screen.width, 100), "GAME OVER", style);
                GUI.Label(new Rect(0, Screen.height / 2 + 20, Screen.width, 60), "The corn was destroyed!", smallStyle);
            }
            
            // Restart instruction
            GUI.Label(new Rect(0, Screen.height / 2 + 100, Screen.width, 50), "Press R to Restart", smallStyle);
        }
    }
    
    void LoseGame()
    {
        gameEnded = true;
        playerWon = false;
        
        Debug.Log("GAME OVER! Corn was destroyed!");
        
        Time.timeScale = 0f; // Pauses the game
    }
    
    void WinGame()
    {
        gameEnded = true;
        playerWon = true;
        
        Debug.Log("VICTORY! All waves defeated!");
        
        Time.timeScale = 0f; // Pauses the game
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