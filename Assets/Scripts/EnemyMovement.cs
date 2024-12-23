using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 1f;

    Rigidbody2D _rb;

    BoxCollider2D _bodyCollider;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bodyCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.linearVelocity = new Vector2(moveSpeed, _rb.linearVelocity.y);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyXAxis();
    }

    private void FlipEnemyXAxis()
    {
        var signX = Mathf.Sign(_rb.linearVelocity.x);
        var currentScale = new Vector3(signX * -1, transform.localScale.y, transform.localScale.z);
        transform.localScale = currentScale;
    }
}
