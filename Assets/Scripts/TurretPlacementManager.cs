using UnityEngine;
using UnityEngine.InputSystem;

public class TurretPlacementManager : MonoBehaviour 
{
    [Header("Turret Prefabs")]
    public GameObject turret1Prefab;
    public GameObject turret2Prefab;
    public GameObject turret3Prefab;
    
    [Header("Settings")]
    public float gridSize = 2f; // Adjust based on your turret size
    
    // State
    private bool isPlacingTurret = false;
    private GameObject ghostTurret;
    private GameObject selectedTurretPrefab;
    private Vector2 lastMovementInput = Vector2.zero;
    
    // Input System reference
    private PlayerInput controls;
    
    void Awake() 
    {
        controls = new PlayerInput();
    }
    
    void OnEnable() 
    {
        // Enable the TurretPlacement action map
        controls.TurretPlacement.Enable();
        
        // Subscribe to turret selection buttons
        controls.TurretPlacement.SelectTurret1.performed += OnSelectTurret1;
        controls.TurretPlacement.SelectTurret2.performed += OnSelectTurret2;
        controls.TurretPlacement.SelectTurret3.performed += OnSelectTurret3;
    }
    
    void OnDisable() 
    {
        // Cleanup
        controls.TurretPlacement.Disable();
        
        // Unsubscribe
        controls.TurretPlacement.SelectTurret1.performed -= OnSelectTurret1;
        controls.TurretPlacement.SelectTurret2.performed -= OnSelectTurret2;
        controls.TurretPlacement.SelectTurret3.performed -= OnSelectTurret3;
    }
    
    void Update() 
    {
        // Handle ghost turret movement
        if (isPlacingTurret && ghostTurret != null) 
        {
            // Read arrow key input
            Vector2 movement = controls.TurretPlacement.MoveCursor.ReadValue<Vector2>();
            
            // Only move when input CHANGES (key press detection, not continuous hold)
            if (movement != lastMovementInput && movement != Vector2.zero) 
            {
                // Move exactly one grid space
                Vector3 gridMove = new Vector3(
                    movement.x * gridSize, 
                    movement.y * gridSize, 
                    0
                );
                
                ghostTurret.transform.position += gridMove;
                
                Debug.Log("Moved ghost by one grid space to: " + ghostTurret.transform.position);
            }
            
            lastMovementInput = movement;
        }
    }
    
    // Event handlers for turret selection
    void OnSelectTurret1(InputAction.CallbackContext context) 
    {
        if (!isPlacingTurret) StartPlacing(turret1Prefab);
    }
    
    void OnSelectTurret2(InputAction.CallbackContext context) 
    {
        if (!isPlacingTurret) StartPlacing(turret2Prefab);
    }
    
    void OnSelectTurret3(InputAction.CallbackContext context) 
    {
        if (!isPlacingTurret) StartPlacing(turret3Prefab);
    }
    
    void StartPlacing(GameObject turretPrefab) 
    {
        if (turretPrefab == null) 
        {
            Debug.LogWarning("Turret prefab is null! Make sure to assign it in the Inspector.");
            return;
        }
        
        isPlacingTurret = true;
        selectedTurretPrefab = turretPrefab;
        
        // Subscribe to placement controls
        controls.TurretPlacement.Confirm.performed += OnConfirmPlacement;
        controls.TurretPlacement.Cancel.performed += OnCancelPlacement;
        
        // Find the player and spawn above them
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 startPosition;
        
        if (player != null)
        {
            // Spawn 4 units above the player (adjust as needed)
            startPosition = player.transform.position + new Vector3(0, 4f, 0);
        }
        else
        {
            // Fallback: spawn at camera center + offset
            startPosition = Camera.main.transform.position + new Vector3(0, 2f, 0);
            Debug.LogWarning("Player not found! Make sure Player GameObject has 'Player' tag.");
        }
        
        startPosition.z = 0; // Make sure it's at z=0 for 2D
        startPosition = SnapToGrid(startPosition); // Start on grid
        
        ghostTurret = Instantiate(turretPrefab, startPosition, Quaternion.identity);
        
        // Make ghost semi-transparent
        SpriteRenderer sr = ghostTurret.GetComponent<SpriteRenderer>();
        if (sr != null) 
        {
            Color color = sr.color;
            color.a = 0.5f; // 50% transparent
            sr.color = color;
        }
        
        Debug.Log("Placement mode started. Use arrow keys to move, Space to confirm, Escape to cancel.");
        Debug.Log("Ghost spawned at: " + startPosition);
    }
    
    // Snap position to nearest grid point
    Vector3 SnapToGrid(Vector3 position) 
    {
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        
        return new Vector3(snappedX, snappedY, position.z);
    }
    
    void OnConfirmPlacement(InputAction.CallbackContext context) 
    {
        if (!isPlacingTurret) return;
        
        // Place the real turret at ghost position
        Instantiate(selectedTurretPrefab, ghostTurret.transform.position, Quaternion.identity);
        
        Debug.Log("Turret placed at " + ghostTurret.transform.position);
        
        ExitPlacementMode();
    }
    
    void OnCancelPlacement(InputAction.CallbackContext context) 
    {
        if (!isPlacingTurret) return;
        
        Debug.Log("Placement cancelled.");
        
        ExitPlacementMode();
    }
    
    void ExitPlacementMode() 
    {
        // Destroy ghost
        if (ghostTurret != null) 
        {
            Destroy(ghostTurret);
        }
        
        // Unsubscribe from placement controls
        controls.TurretPlacement.Confirm.performed -= OnConfirmPlacement;
        controls.TurretPlacement.Cancel.performed -= OnCancelPlacement;
        
        // Reset state
        isPlacingTurret = false;
        ghostTurret = null;
        lastMovementInput = Vector2.zero; // Reset movement tracking
    }
    
    // Public getter for other scripts
    public bool IsPlacing() 
    {
        return isPlacingTurret;
    }
}