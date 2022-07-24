using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    
    public float speed = 8f;
    public float jumpForce = 8f;
    public float doubleJumpForce = 4f;
    private int _jumpCount;
    private bool _isOnWall;
    private bool _isDashing;
    private bool _canDash;
    
    private bool _facingRight;
    private Rigidbody2D _player;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _playerCol;
    private BetterJump _betterJump;
    private Animator _animator;

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
        _betterJump = GetComponent<BetterJump>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        underRoof = Physics2D.Raycast(groundCheck.position, Vector2.up, 3f, roofLayer);

        if (isGrounded)
        {
            _animator.SetBool("IsGrounded", true);
        }
        else
        {
            _animator.SetBool("IsGrounded", false);
        }
        
        var x = inputManager.walkInput;
        var y = inputManager.jumpInput;
        var dir = new Vector2(x, y);
        
        Dash(x, y);
        Walk(dir);
        Jump();
        Roll();
        Flip();
        WallJump();
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            _canDash = true;
        }
    }

    private void Walk(Vector2 dir)
    {
        if (_isDashing)
        {
            return;
        }
        _player.velocity = new Vector2(dir.x * speed, _player.velocity.y);
        if (_player.velocity.x > 0.01f || _player.velocity.x < 0f)
        {
            _animator.SetFloat("Speed",1);
        }
        else if (_player.velocity.x == 0)
        {
            _animator.SetFloat("Speed",0);
        }
    }

    private void Jump()
    {

        if (_player.velocity.y > 0)
        {
            _animator.SetFloat("JumpVelocity", 1);
        }
        else if (_player.velocity.y < 0)
        {
            _animator.SetFloat("JumpVelocity", -1);

        }
        
        if (Input.GetButtonDown("Jump") && isGrounded && !_isOnWall)
        {
            _player.velocity = new Vector2(_player.velocity.x, 0f);
            _player.velocity += Vector2.up * jumpForce;
            _jumpCount += 1;
            _canDash = false;
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
            _animator.SetBool("IsRolling",true);
            transform.localScale = new Vector3(5.5f, 5.5f, 1);
        }
        else if (!inputManager.exitRollInput && !underRoof)
        {
            _animator.SetBool("IsRolling",false);
            transform.localScale = new Vector3(11,11,1);
        }
    }

    private void WallJump()
    {
        if (_isOnWall && Input.GetButtonDown("Jump"))
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

    private void Dash(float x, float y)
    {
        if (inputManager.dashInput)
        {
            _player.velocity = Vector2.zero;
            Vector2 dir = new Vector2(x, y);

            _player.velocity += dir.normalized * 20;
            StartCoroutine(DashWait());
        }
    }

    IEnumerator DashWait()
    {
        _player.gravityScale = 0f;
        _betterJump.enabled = false;
        _isDashing = true;

        yield return new WaitForSeconds(0.2f);

        _isDashing = false;
        _player.gravityScale = 2.6f;
        _betterJump.enabled = true;
    }
}