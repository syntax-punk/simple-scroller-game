using System;
using TMPro;
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
    private Animator _animator;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Run();
    }

    private void Run()
    {
        var currentVelocity = new Vector2(_moveInput.x * runSpeed, _rb.linearVelocityY);
        _rb.linearVelocity = currentVelocity;

        var playerIsMovingHorizontally = Mathf.Abs(_moveInput.x) > Mathf.Epsilon;

        if (playerIsMovingHorizontally)
        {
            FlipSprite();
        }

        _animator.SetBool("isRunning", playerIsMovingHorizontally);
    }

    private void FlipSprite()
    {
        var signX = Mathf.Sign(_moveInput.x);
        var currentScale = new Vector3(signX, transform.localScale.y, transform.localScale.z);
        transform.localScale = currentScale;
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
}
