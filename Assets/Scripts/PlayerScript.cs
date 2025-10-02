using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D _rbody;
    public float speed = 5f;
    private Vector2 _moveDirection;
    private TurretPlacementManager _placementManager;

    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _moveDirection = Vector2.zero;
        
        // Find the placement manager in the scene
        _placementManager = FindFirstObjectByType<TurretPlacementManager>();
        
        if (_placementManager == null)
        {
            Debug.LogWarning("TurretPlacementManager not found in scene!");
        }
    }

    private void FixedUpdate()
    {
        // Only move if NOT in placement mode
        if (_placementManager == null || !_placementManager.IsPlacing())
        {
            _rbody.linearVelocity = _moveDirection * speed;
        }
        else
        {
            // Stop player movement when placing turrets
            _rbody.linearVelocity = Vector2.zero;
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 moveVec = value.Get<Vector2>();
        _moveDirection = moveVec;
    }
}