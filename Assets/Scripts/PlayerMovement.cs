using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float runSpeed = 6f;

    [SerializeField]
    float runBoost = 20f;

    [SerializeField]
    float jumpForce = 5f;

    [SerializeField]
    float climbSpeed = 6f;

    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private Animator _animator;
    private CapsuleCollider2D _bodyCollider;
    private BoxCollider2D _feetCollider;
    private float _gravityScaleAtStart;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
        _feetCollider = GetComponent<BoxCollider2D>();

        _gravityScaleAtStart = _rb.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        Climb();
    }

    private void Run()
    {
        var movementVelocity = new Vector2(_moveInput.x * runSpeed, _rb.linearVelocityY);
        _rb.linearVelocity = movementVelocity;

        var playerIsMovingHorizontally = Mathf.Abs(_moveInput.x) > Mathf.Epsilon;

        _animator.SetBool("isRunning", playerIsMovingHorizontally);
    }

    private void Climb()
    {
        var climbingLayer = LayerMask.GetMask("Climbing");
        if (!_feetCollider.IsTouchingLayers(climbingLayer))
        {
            if (_rb.gravityScale != _gravityScaleAtStart)
            {
                _rb.gravityScale = _gravityScaleAtStart;
                _animator.SetBool("isClimbing", false);
            }
            return;
        }

        var playerMovingVertically = Mathf.Abs(_moveInput.y) > Mathf.Epsilon;
        var climbVelocity = new Vector2(_rb.linearVelocity.x, _moveInput.y * climbSpeed);

        _animator.SetBool("isClimbing", playerMovingVertically);
        _rb.gravityScale = 0;
        _rb.linearVelocity = climbVelocity;
    }

    private void FlipSprite()
    {
        var playerIsMovingHorizontally = Mathf.Abs(_moveInput.x) > Mathf.Epsilon;
        if (!playerIsMovingHorizontally) return;

        var signX = Mathf.Sign(_moveInput.x);
        var currentScale = new Vector3(signX, transform.localScale.y, transform.localScale.z);
        transform.localScale = currentScale;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Ladder");
        }
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        var groundLayer = LayerMask.GetMask("Ground");

        if (!_feetCollider.IsTouchingLayers(groundLayer)) return;

        if (value.isPressed)
        {
            _rb.linearVelocityY += jumpForce;
        }
    }
}
