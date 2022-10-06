using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private InputManager _inputManager;
    public UnitSO Unit;

    [SerializeField] private ParticleSystem _dust;
    
    public float Speed = 8f;
    public float JumpForce = 8f;
    public float DoubleJumpForce = 8f;
    public float SlideSpeed;
    public float GlideSpeed;
    private bool _isOnWall;
    public bool _isDashing { get; private set; }
    private bool _canDash;
    private bool _hasWallJumped;
    private bool _isGliding;
    private float _rollInterpolator;
    private bool _doubleJump;
    
    private bool _facingRight;
    [SerializeField] private GameObject weapon;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private BetterJump _betterJump;
    private Animator _animator;

    public Transform GroundChecker;
    public float GroundCheckRadius;
    public LayerMask GroundLayer;
    public LayerMask RoofLayer;
    public bool IsGrounded;
    public bool UnderRoof;
    public float GroundedRememberTime;
    private float _coyoteTime;
    public float JumpRememberTime;
    private float _jumpRemember;

    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _betterJump = GetComponent<BetterJump>();
        _animator = GetComponent<Animator>();
        _inputManager = GetComponent<InputManager>();
        _canDash = true;
    }


    private void Update()
    {
        var x = _inputManager.walkInput;
        var y = _inputManager.jumpInput;
        var dir = new Vector2(x, y);
        
        SurfaceChecks();
        if (Unit.HasDash)
            Dash(x, y);
        
        if (Unit.HasGlide)
            Glide();
        
        if (Unit.HasWalk)
            Walk(dir);
        
        if (Unit.HasJump)
            Jump();

        if (Unit.HasRoll)
            Roll();
        
        if (Unit.HasWallJump)
            WallJump(dir);
        
        Flip();
    }
    
    

    private void SurfaceChecks()
    {
        UnderRoof = Physics2D.Raycast(GroundChecker.position, Vector2.up, 3f, RoofLayer);
        IsGrounded = Physics2D.OverlapCircle(GroundChecker.position, GroundCheckRadius, GroundLayer);

        _coyoteTime -= Time.deltaTime;
        
        if (IsGrounded)
        {
            _animator.SetBool("IsGrounded", true);
            _coyoteTime = GroundedRememberTime;
        }
        
        else
            _animator.SetBool("IsGrounded", false);

        _jumpRemember -= Time.deltaTime;
        
        
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            _jumpRemember = JumpRememberTime;
            _doubleJump = true;
        }
    }

    
    
    private void Walk(Vector2 dir)
    {
        if (_isDashing)
            return;

        if (_hasWallJumped)
            return;
        
        _rb.velocity = new Vector2(dir.x * Speed, _rb.velocity.y);
        
        if (_rb.velocity.x > 0.01f || _rb.velocity.x < 0f)
            _animator.SetFloat("Speed",1);
        
        else if (_rb.velocity.x == 0)
            _animator.SetFloat("Speed",0);
    }

    
    
    private void Jump()
    {
        
        if (IsGrounded && !Input.GetButton("Jump"))
            _doubleJump = false;

        if (_rb.velocity.y > 0)
            _animator.SetFloat("JumpVelocity", 1);
        
        else if (_rb.velocity.y < 0)
            _animator.SetFloat("JumpVelocity", -1);

        if ((_coyoteTime > 0) && (_jumpRemember > 0) && !_isOnWall)
            _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);
        
        else if (_doubleJump && Input.GetButtonDown("Jump") && Unit.HasDoubleJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, DoubleJumpForce);
            _doubleJump = false;
        }


        if (Input.GetButtonDown("Jump") && IsGrounded)
            CreateDust();
    }

    
    
    private void Flip()
    {
        if (_inputManager.walkInput > 0f && _facingRight)
        {
            if(IsGrounded)
                CreateDust();
            _facingRight = !_facingRight;
            _spriteRenderer.flipX = false;
            weapon.transform.localPosition = new Vector2(0.2f, 0);
            weapon.transform.localScale = new Vector3(0.35f, 0.05f);
        }
        
        else if (_inputManager.walkInput < 0f && !_facingRight)
        {
            if(IsGrounded)
                CreateDust();
            _facingRight = !_facingRight;
            _spriteRenderer.flipX = true;
            weapon.transform.localPosition = new Vector2(-0.2f, 0);
            weapon.transform.localScale = new Vector3(0.35f, 0.05f);
        }
        
        else if (_inputManager.upInput)
        {
            weapon.transform.localScale = new Vector3(0.05f, 0.35f);
            weapon.transform.localPosition = new Vector3(0f, 0.15f);
        }
    }

    
    
    private void Roll()
    {
        if (_inputManager.rollInput && IsGrounded)
        {
            _animator.SetBool("IsRolling",true);
            transform.localScale = new Vector3(9f, 9f, 1);
            if (_rb.velocity.x > 0 || _rb.velocity.x < 0)
            {
                Speed = Mathf.Lerp(4f, 12f, _rollInterpolator);
                _rollInterpolator += 0.4f * Time.deltaTime;
            }

        }
        else if (!_inputManager.exitRollInput && !UnderRoof || _isOnWall)
        {
            _animator.SetBool("IsRolling",false);
            transform.localScale = new Vector3(11,11,1);
            Speed = 8f;
            _rollInterpolator = 0f;
        }
    }

    
    
    private void WallJump(Vector2 dir)
    {
        if (_isOnWall && Input.GetButtonDown("Jump") && _rb.velocity.x < 0f || _isOnWall && Input.GetButtonDown("Jump") && 0f < _rb.velocity.x)
        {
            _rb.velocity = new Vector2((dir.x * 2) * Speed * 0.7f, JumpForce);
            _hasWallJumped = true;
            _doubleJump = true;
            StartCoroutine(WallJumpTimer());
        }
    }

    
    
    private void OnCollisionStay2D(Collision2D col)
    {
        if (Unit.HasWallJump)
        {
            if (col.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                _rb.velocity = new Vector2(_rb.velocity.x, SlideSpeed * Time.deltaTime);
                _isOnWall = true;
                _animator.SetBool("IsOnWall", true);
            }
        }
    }

    
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Wind") && _isGliding)
            _rb.velocity = new Vector2(_rb.velocity.x, GlideSpeed * Time.deltaTime);
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


    
    private IEnumerator WallJumpTimer()
    {
        yield return new WaitForSeconds(0.25f);
        _isOnWall = false;

        yield return new WaitForSeconds(0.25f);
            
        _hasWallJumped = false;
    }

    
    
    private void Dash(float x, float y)
    {
        if (_inputManager.dashInput && _canDash)
        {
            _rb.velocity = Vector2.zero;
            Vector2 dir = new Vector2(x, y);

            _rb.velocity += dir.normalized * 20;
            StartCoroutine(DashWait());
        }
    }


    private IEnumerator DashWait()
    {
        _rb.gravityScale = 0f;
        _betterJump.enabled = false;
        _isDashing = true;
        _canDash = false;

        yield return new WaitForSeconds(0.2f);

        _isDashing = false;
        _rb.gravityScale = 2.6f;
        _betterJump.enabled = true;

        yield return new WaitForSeconds(1f);

        if (IsGrounded)
            _canDash = true;
        
        else if (!IsGrounded)
        {
            while (!IsGrounded)
                yield return this;

            _canDash = true;
        }
    }

    
    
    private void Glide()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _betterJump.enabled = false;
            _isGliding = true;
            _rb.velocity = new Vector2(0f, -1f);

            if (Input.GetKey(KeyCode.Mouse1))
                _rb.gravityScale = 0.4f;
        }

        
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _rb.gravityScale = 2.6f;
            _betterJump.enabled = true;
            _isGliding = false;
        }
    }

    
    private void CreateDust() => _dust.Play();
}