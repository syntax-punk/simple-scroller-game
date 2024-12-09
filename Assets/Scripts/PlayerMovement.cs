using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float runSpeed = 6f;

    [SerializeField]
    float runBoost = 20f;

    [SerializeField]
    float gravity = 1f;


    private Vector2 _moveInput;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Run();
    }

    private void Run()
    {
        var currentVelocity = new Vector2(_moveInput.x * runSpeed, _rb.linearVelocityY);
        _rb.linearVelocity = currentVelocity;
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        Debug.Log(_moveInput);
    }
}
