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
    public float slideSpeed;
    private bool _isOnWall;
    private bool _isDashing;
    private bool _canDash;
    private bool _hasWallJumped;
    private float _rollInterpolator;
    
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
    public float _groundedRememberTime;
    private float _coyoteTime;
    public float _jumpRememberTime;
    private float _jumpRemember;
    
    void Start()
    {
        _player = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerCol = GetComponent<CircleCollider2D>();
        _betterJump = GetComponent<BetterJump>();
        _animator = GetComponent<Animator>();
        _canDash = true;
    }

    void Update()
    {

        var x = inputManager.walkInput;
        var y = inputManager.jumpInput;
        var dir = new Vector2(x, y);
        
        SurfaceChecks();
        Dash(x, y);
        Glide();
        Walk(dir);
        Jump();
        Roll();
        Flip();
        WallJump(dir);
    }

    private void SurfaceChecks()
    {
        underRoof = Physics2D.Raycast(groundCheck.position, Vector2.up, 3f, roofLayer);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        _coyoteTime -= Time.deltaTime;
        if (isGrounded)
        {
            _animator.SetBool("IsGrounded", true);
            _coyoteTime = _groundedRememberTime;
        }
        else
        {
            _animator.SetBool("IsGrounded", false);
        }

        _jumpRemember -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            _jumpRemember = _jumpRememberTime;
        }
    }

    private void Walk(Vector2 dir)
    {
        if (_isDashing)
        {
            return;
        }
        
        if (_hasWallJumped)
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
        
        if ((_coyoteTime > 0) && (_jumpRemember > 0) && !_isOnWall)
        {
            _player.velocity = new Vector2(_player.velocity.x, jumpForce);
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
            _animator.SetBool("IsRolling",true);
            transform.localScale = new Vector3(9f, 9f, 1);
            if (_player.velocity.x > 0)
            {
                speed = Mathf.Lerp(4f, 12f, _rollInterpolator);
                _rollInterpolator += 0.4f * Time.deltaTime;
            }

        }
        else if (!inputManager.exitRollInput && !underRoof || _isOnWall)
        {
            _animator.SetBool("IsRolling",false);
            transform.localScale = new Vector3(11,11,1);
            speed = 8f;
            _rollInterpolator = 0f;
        }
    }

    private void WallJump(Vector2 dir)
    {
        if (_isOnWall && Input.GetButtonDown("Jump") && _player.velocity.x < 0f || _isOnWall && Input.GetButtonDown("Jump") && 0f < _player.velocity.x)
        {
            _player.velocity = new Vector2(dir.x * speed * 3.6f, jumpForce);
            _jumpCount = 0;
            _hasWallJumped = true;
            StartCoroutine(WallJumpTimer());
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _player.velocity = new Vector2(_player.velocity.x, slideSpeed * Time.deltaTime);
            _isOnWall = true;
            _animator.SetBool("IsOnWall", true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _canDash = true;
            StartCoroutine(WallJumpTimer());
            _animator.SetBool("IsOnWall", false);
        }
    }

    IEnumerator WallJumpTimer()
    {
        yield return new WaitForSeconds(0.25f);
        _isOnWall = false;

        yield return new WaitForSeconds(0.25f);
            
        _hasWallJumped = false;
    }

    private void Dash(float x, float y)
    {
        if (inputManager.dashInput && _canDash)
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
        _canDash = false;

        yield return new WaitForSeconds(0.2f);

        _isDashing = false;
        _player.gravityScale = 2.6f;
        _betterJump.enabled = true;

        yield return new WaitForSeconds(1f);

        if (isGrounded)
        {
            _canDash = true;
        }
        else if (!isGrounded)
        {
            while (!isGrounded)
            {
                yield return this;
            }

            _canDash = true;
        }

    }

    private void Glide()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _player.velocity = Vector2.zero;
            }
            _player.gravityScale = 0.2f;
            _betterJump.enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _player.gravityScale = 2.6f;
            _betterJump.enabled = true;
        }

    }
}