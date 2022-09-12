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
    [SerializeField] private ParticleSystem _dashTrail;
    private bool _groundTouched = false;
    private bool _hasCreatedDust;
    
    public float slideSpeed;
    public float glideSpeed;
    private bool _isOnWall;
    
    [Header("MOVEMENT")]
    [SerializeField] private float _movementAcceleration = 5f;
    [SerializeField] private float _maxMoveSpeed = 15f;
    [SerializeField] private float _linearDrag = 5f;
    private float _horizontalDirection;
    
    [Header("JUMP")]
    private bool _isJumping = false; //NEW
    public float jumpForce = 8f;

    [Header("Dash")]
    [SerializeField] private float _dashMultiplier;
    [SerializeField] private float _dashShakeIntensity;
    [SerializeField] private float _dashShakeDuration;
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
    
    //GRUNDCHECK
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
        ApplyLinearDrag(); //NEW
        AnimationCheck();
        DustOnLand();//NEW
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
        
    }   //NEW
    
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
        
        //NEW MOVEMENT
        _player.AddForce((dir * _movementAcceleration) * Time.deltaTime);
        if (Mathf.Abs(_player.velocity.x) > _maxMoveSpeed)
            _player.velocity = new Vector2(Mathf.Sign(_player.velocity.x) * _maxMoveSpeed, _player.velocity.y);
        
        //_player.velocity = new Vector2(dir.x * speed, _player.velocity.y);
        /*
        if (_player.velocity.x > 0.01f || _player.velocity.x < 0f)
        {
            _animator.SetFloat("Speed",1);
        }
        else if (_player.velocity.x == 0)
        {
            _animator.SetFloat("Speed",0);
        }
        */
    }

    private void Jump() //NEW
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
        float moveSpeedTemp = _maxMoveSpeed;
        if (inputManager.rollhInput && isGrounded)
        {
            _animator.SetBool("IsRolling",true);
            transform.localScale = new Vector3(9f, 9f, 1);
            if (_player.velocity.x > 0 || _player.velocity.x < 0)
            {
                _maxMoveSpeed = Mathf.Lerp(4f, 12f, _rollInterpolator);
                _rollInterpolator += 0.4f * Time.deltaTime;
            }

        }
        else if (!inputManager.exitRollInput && !underRoof || _isOnWall)
        {
            _animator.SetBool("IsRolling",false);
            transform.localScale = new Vector3(11,11,1);
            _maxMoveSpeed = moveSpeedTemp;
            _rollInterpolator = 0f;
        }
    }

    private void WallJump(Vector2 dir)
    {
        if (_isOnWall && Input.GetButtonDown("Jump") && _player.velocity.x < 0f || _isOnWall && Input.GetButtonDown("Jump") && 0f < _player.velocity.x)
        {
            _player.velocity = new Vector2((dir.x * 2) * _maxMoveSpeed * 1.5f, jumpForce * 2f);
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
            //FEEDBACK
            CreateDashTrail();
            VirtualMachineShake.Instance.CameraShake(_dashShakeIntensity, _dashShakeDuration);
            
            _player.velocity = Vector2.zero;
            Vector2 dir = new Vector2(x, y);

            _player.velocity += dir.normalized * _dashMultiplier;
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
    
    void CreateDashTrail()
    {
        _dashTrail.Play();
    }
    
    void ApplyLinearDrag() //NEW
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f)
            _player.drag = _linearDrag;
        else
            _player.drag = 0;
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

    void DustOnLand()
    {
        if(!isGrounded)
        {
            _groundTouched = false;
            _hasCreatedDust = false;
        }
        else
            _groundTouched = true;

        if(_groundTouched && !_hasCreatedDust)
        {
            CreateDust();
            _groundTouched = false;
            _hasCreatedDust = true;
        } 
    }
    
}
