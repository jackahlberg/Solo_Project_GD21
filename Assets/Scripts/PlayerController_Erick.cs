using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController_Erick : MonoBehaviour
{
    private InputManagerErick inputManager;
    public UnitSO unit;

    [SerializeField] private ParticleSystem _dust;
    
    public float speed = 8f;
    public float slideSpeed;
    public float glideSpeed;
    private bool _isOnWall;
    
    [Header("JUMP")]
    private bool _isJumping = false; //NEW
    public float jumpForce = 8f;
    public bool isDashing { get; private set; }
    private bool _canDash;
    private bool _hasWallJumped;
    private bool _isGliding;
    private float _rollInterpolator;
    
    private bool _facingRight;
    
    [Header("REFERENCES")]
    [SerializeField] private GameObject weapon;
    private Rigidbody2D _player;
    private SpriteRenderer _spriteRenderer;
    private BetterJumpErick _betterJump;

    //ANIMATION
    private Animator _animator;
    private string _currentState;
    
    //ANIMATION STATES
    private const string PlayerIdle = "idle";
    private const string PlayerWalk = "walk";
    private const string PlayerJump = "jump";
    private const string PlayerFall = "fall";
    private const string PlayerRoll = "roll";
    
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
        _betterJump = GetComponent<BetterJumpErick>();
        _animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManagerErick>();
        _canDash = true;
        groundCheck = gameObject.GetComponent<Transform>(); //NEW
    }

    void Update()
    {
        var x = inputManager.walkInput;
        var y = Input.GetAxis("Vertical"); //NEW
        var dir = new Vector2(x, 0);
        
        SurfaceChecks();
        if (unit.hasDash)
        {
            Dash(x, y);
        }
        if (unit.hasGlide)
        {
            Glide();
        }
        if (unit.hasWalk)
        {
            Walk(dir);
        }

        if (unit.hasJump)
        {
            Jump();
        }

        if (unit.hasRoll)
        {
            Roll();
        }
        Flip();
        if (unit.hasWallJump)
        {
            WallJump(dir);
        }

        AnimationCheck();
    }

    void AnimationCheck()
    {
        if (isGrounded)
        {
            _isJumping = false;
            if (inputManager.walkInput != 0)
                ChangeAnimationState(PlayerWalk);
            else
                ChangeAnimationState(PlayerIdle);
        }
        else
        {
            if (_player.velocity.y > 0) 
                ChangeAnimationState(PlayerJump);

            if (_player.velocity.y <= 0)
                ChangeAnimationState(PlayerFall); 
        }
        
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
        if (isDashing)
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
        if (inputManager.jumpInput && isGrounded)
        {
            CreateDust();
            _player.velocity = new Vector2(_player.velocity.x, 0);
            _player.velocity = new Vector2(_player.velocity.x, jumpForce);
            isGrounded = false;
            _isJumping = true;
        }
        
        /*
        if (_player.velocity.y > 0)
        {
            _animator.SetFloat("JumpVelocity", 1);
        }
        else if (_player.velocity.y < 0)
        {
            _animator.SetFloat("JumpVelocity", -1);
        }
        */
        
        if ((_coyoteTime > 0) && (_jumpRemember > 0) && !_isOnWall)
        {
            _player.velocity = new Vector2(_player.velocity.x, jumpForce);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            CreateDust();
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
            if(isGrounded)
                CreateDust();
            _facingRight = !_facingRight;
            _spriteRenderer.flipX = false;
            weapon.transform.localPosition = new Vector2(0.2f, 0);
            weapon.transform.localScale = new Vector3(0.35f, 0.05f);
        }
        else if (inputManager.walkInput < 0f && !_facingRight)
        {
            if(isGrounded)
                CreateDust();
            _facingRight = !_facingRight;
            _spriteRenderer.flipX = true;
            weapon.transform.localPosition = new Vector2(-0.2f, 0);
            weapon.transform.localScale = new Vector3(0.35f, 0.05f);
        }
        else if (inputManager.upInput)
        {
            weapon.transform.localScale = new Vector3(0.05f, 0.35f);
            weapon.transform.localPosition = new Vector3(0f, 0.15f);
        }
    }

    private void Roll()
    {
        if (inputManager.rollhInput && isGrounded)
        {
            _animator.SetBool("IsRolling",true);
            transform.localScale = new Vector3(9f, 9f, 1);
            if (_player.velocity.x > 0 || _player.velocity.x < 0)
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
            _player.velocity = new Vector2((dir.x * 2) * speed * 0.7f, jumpForce);
            _hasWallJumped = true;
            StartCoroutine(WallJumpTimer());
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (unit.hasWallJump)
        {
            if (col.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                _player.velocity = new Vector2(_player.velocity.x, slideSpeed * Time.deltaTime);
                _isOnWall = true;
                _animator.SetBool("IsOnWall", true);
            }
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Wind") && _isGliding)
        {
            _player.velocity = new Vector2(_player.velocity.x, glideSpeed * Time.deltaTime);
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
        isDashing = true;
        _canDash = false;

        yield return new WaitForSeconds(0.2f);

        isDashing = false;
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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _betterJump.enabled = false;
            _isGliding = true;
            _player.velocity = new Vector2(0f, -1f);

            if (Input.GetKey(KeyCode.Mouse1))
            {
                _player.gravityScale = 0.4f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _player.gravityScale = 2.6f;
            _betterJump.enabled = true;
            _isGliding = false;
        }
    }
    
    void CreateDust()
    {
        _dust.Play();
    }
    
    void ChangeAnimationState(string newState) //NEW
    {
        //stop the same animation from interrupting itself
        if (_currentState == newState) return;
        
        //play the animation
        _animator.Play(newState);
        
        //reassigning the current state
        _currentState = newState;
    }
}
