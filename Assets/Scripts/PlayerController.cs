using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    
    public float speed = 5f;
    public float jumpForce = 8f;
    public float doubleJumpForce = 4f;
    private int _jumpCount;
    private bool _isOnWall;
    
    private bool _facingRight;
    private Rigidbody2D _player;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _playerCol;
    [SerializeField] private Sprite rollSprite;
    [SerializeField] private Sprite mainSprite;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public LayerMask roofLayer;
    public bool isGrounded;
    public bool underRoof;
    void Start()
    {
        _player = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerCol = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        underRoof = Physics2D.Raycast(groundCheck.position, Vector2.up, 3f, roofLayer);

        float x = inputManager.walkInput;
        float y = _player.velocity.y;
        Vector2 dir = new Vector2(x, y);
        
        Dash();
        Walk(dir);
        Jump();
        Roll();
        Flip();
        WallJump();
    }

    private void Walk(Vector2 dir)
    {
        _player.velocity = new Vector2(dir.x * speed, _player.velocity.y); 
    }

    private void Jump()
    {
        if (inputManager.jumpInput && isGrounded && !_isOnWall)
        {
            _player.velocity = new Vector2(_player.velocity.x, 0f);
            _player.velocity += Vector2.up * jumpForce;
            _jumpCount += 1;
        }
        //Double Jump
        /*else if (inputManager.jumpInput && _jumpCount == 1 && !_isOnWall)
        {
            _player.velocity = new Vector2(_player.velocity.x, doubleJumpForce);
            _jumpCount = 0;
        }*/
    }

    private void Flip()
    {
        if (inputManager.walkInput > 0f && _facingRight)
        {
            _facingRight = !_facingRight;
            _spriteRenderer.flipX = false;
        }
        else if (inputManager.walkInput < 0f && !_facingRight)
        {
            _facingRight = !_facingRight;
            _spriteRenderer.flipX = true;

        }
    }

    private void Roll()
    {
        if (inputManager.rollhInput && isGrounded)
        {
            _spriteRenderer.sprite = rollSprite;
            _playerCol.radius = 0.048f;
            speed = 3f;
        }
        else if (!inputManager.exitRollInput && !underRoof)
        {
            _spriteRenderer.sprite = mainSprite;
            _playerCol.radius = 0.1f;
            speed = 5f;
        }
    }

    private void WallJump()
    {
        if (_isOnWall && inputManager.jumpInput)
        {
            _player.velocity = new Vector2(inputManager.walkInput, jumpForce);
            _jumpCount = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _player.velocity = Vector2.zero;
            _isOnWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            StartCoroutine(WallJumpTimer());
        }
    }

    IEnumerator WallJumpTimer()
    {
        yield return new WaitForSeconds(0.25f);
        _isOnWall = false;
    }

    private void Dash()
    {
        if (inputManager.dashInput)
        {
            Debug.Log("Dash");
            //_player.velocity = new Vector2(); FIX THIS LATER
        }
    }
}