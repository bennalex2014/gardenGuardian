using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D _rbody;
    public float speed = 5f;
    private Vector2 _moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _moveDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        _rbody.linearVelocity = _moveDirection * speed;
    }

    void OnMove(InputValue value)
    {
        Vector2 moveVec = value.Get<Vector2>();
        _moveDirection = moveVec;
    }

}
