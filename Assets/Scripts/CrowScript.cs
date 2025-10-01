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
    public int health = 100;

    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 targetPosition;

        if (_playerTransform != null)
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
}
