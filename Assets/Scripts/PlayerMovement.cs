using System;
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

    [SerializeField]
    Vector2 DeathKick = new Vector2(8f, 8f);

    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private Animator _animator;
    private CapsuleCollider2D _bodyCollider;
    private BoxCollider2D _feetCollider;
    private float _gravityScaleAtStart;
    private bool _isAlive = true;
    private CameraShake _cameraShake;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
        _feetCollider = GetComponent<BoxCollider2D>();

        _gravityScaleAtStart = _rb.gravityScale;
        _cameraShake = GetCameraShake();
    }

    void Update()
    {
        if (!_isAlive) return;

        Run();
        FlipSprite();
        Climb();
        Die();
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

    private void Die()
    {
        if (_bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            _isAlive = false;
            _animator.SetTrigger("Dying");

            if (_cameraShake != null)
                _cameraShake.TriggerShake();

            _rb.linearVelocity = DeathKick;
        }
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
        if (!_isAlive) return;

        _moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!_isAlive) return;

        var groundLayer = LayerMask.GetMask("Ground");

        if (!_feetCollider.IsTouchingLayers(groundLayer)) return;

        if (value.isPressed)
        {
            _rb.linearVelocityY += jumpForce;
        }
    }

    private CameraShake GetCameraShake()
    {
        var globalCamera = GameObject.FindWithTag("GlobalCameras");

        if (globalCamera == null)
        {
            Debug.LogError("GlobalCameras object not found in the scene");
            return null;
        }

        var shake = globalCamera.GetComponent<CameraShake>();

        return shake;
    }
}
