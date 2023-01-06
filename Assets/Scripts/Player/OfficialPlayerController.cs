using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class OfficialPlayerController : MonoBehaviour
{
    private InputManager _inputManager;
    public UnitSO Unit;
    public StatsSO Stats;

    [SerializeField] private ParticleSystem _dust;
    [SerializeField] private ParticleSystem _dashTrail;
    private bool _groundTouched = false;
    private bool _hasCreatedDust;
    
    public float SlideSpeed;
    public float GlideSpeed;
    private bool _isOnWall;
    
    [Header("MOVEMENT")]
    [SerializeField] private float _movementAcceleration = 5f;
    [SerializeField] private float _maxMoveSpeed = 15f;
    [SerializeField] private float _linearDrag = 5f;
    private float _horizontalDirection;
    
    [Header("JUMP")]
    private bool _isJumping = false; //NEW
    public float JumpForce = 8f;
    public float DoubleJumpForce = 8f;
    public bool _canDoubleJump;

    [Header("Dash")]
    [SerializeField] private float _dashMultiplier;
    [SerializeField] private float _dashShakeIntensity;
    [SerializeField] private float _dashShakeDuration;
    public bool _isDashing { get; private set; }
    private bool _canDash;
    private bool _hasWallJumped;
    private bool _isGliding;
    private float _rollInterpolator;
    
    private bool _facingRight;
    
    [Header("REFERENCES")]
    [SerializeField] private GameObject _weapon;
    private Rigidbody2D _player;
    private SpriteRenderer _spriteRenderer;
    private BetterJump _betterJump;

    //ANIMATION
    private Animator _animator;
    private string _currentState;
    
    //ANIMATION STATES
    private const string PlayerIdle = "idle";
    private const string PlayerWalk = "walk";
    private const string PlayerJump = "jump";
    private const string PlayerFall = "fall";
    private const string PlayerRoll = "roll";
    
    //GROUND CHECK
    public Transform GroundChecker;
    public float GroundCheckRadius;
    public LayerMask GroundLayer;
    public LayerMask RoofLayer;
    [HideInInspector] public bool IsGrounded;
    [HideInInspector] public bool UnderRoof;
    public float GroundedRememberTime;
    private float _coyoteTime;
    public float JumpRememberTime;
    private float _jumpRemember;

    
    private void Start()
    {
        _player = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _betterJump = GetComponent<BetterJump>();
        _animator = GetComponent<Animator>();
        _inputManager = GetComponent<InputManager>();
        _canDash = true;
        GroundChecker = gameObject.GetComponent<Transform>(); //NEW

        JumpForce = Stats.jumpForce;
        DoubleJumpForce = Stats.doubleJumpForce;
        _dashMultiplier = Stats.dashMultiplier;
        _maxMoveSpeed = Stats.movementSpeed;
    }

    
    
    private void Update()
    {
        var x = _inputManager.HorizontalInput;
        var y = _inputManager.VerticalInput;
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
        
        Flip();
        if (Unit.HasWallJump)
            WallJump(dir);
        
        ApplyLinearDrag(); //NEW
        AnimationCheck();
        DustOnLand();//NEW
    }


    private void AnimationCheck()
    {
        if (IsGrounded)
        {
            _isJumping = false;
            if (_inputManager.HorizontalInput != 0)
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
        if (Input.GetButtonDown("Jump"))
            _jumpRemember = JumpRememberTime;
    }

    
    
    private void Walk(Vector2 dir)
    {
        if (_isDashing)
            return;

        if (_hasWallJumped)
            return;

        //NEW MOVEMENT
        _player.AddForce((dir * _movementAcceleration) * Time.deltaTime);
        
        if (Mathf.Abs(_player.velocity.x) > _maxMoveSpeed)
            _player.velocity = new Vector2(Mathf.Sign(_player.velocity.x) * _maxMoveSpeed, _player.velocity.y);
    }

    
    
    private void Jump() //NEW
    {
        if((_coyoteTime > 0) && (_jumpRemember > 0) && !_isOnWall && !_canDoubleJump || IsGrounded && !_canDoubleJump && _inputManager.IsJumping)
        {
            CreateDust();
            _player.velocity = new Vector2(_player.velocity.x, 0);
            _player.velocity = new Vector2(_player.velocity.x, JumpForce);
            _isJumping = true;
            _canDoubleJump = true;
        }
        
        //Double Jump
        else if (_canDoubleJump && _inputManager.JumpDown && !_isOnWall)
        {
            if (!Unit.HasDoubleJump)
            {
                _canDoubleJump = false;
                return;
            }
            _player.velocity = new Vector2(_player.velocity.x, 0);
            _player.velocity = new Vector2(_player.velocity.x, JumpForce);
            _canDoubleJump = false;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded)
            CreateDust();
    }
    
    
    
    private void Flip()
    {
        if (_inputManager.HorizontalInput > 0f && _facingRight)
        {
            if(IsGrounded)
                CreateDust();
            
            _facingRight = !_facingRight;
            _spriteRenderer.flipX = false;
            _weapon.transform.localPosition = new Vector2(0.2f, 0);
            _weapon.transform.localScale = new Vector3(0.35f, 0.05f);
        }
        
        else if (_inputManager.HorizontalInput < 0f && !_facingRight)
        {
            if(IsGrounded)
                CreateDust();
            _facingRight = !_facingRight;
            _spriteRenderer.flipX = true;
            _weapon.transform.localPosition = new Vector2(-0.2f, 0);
            _weapon.transform.localScale = new Vector3(0.35f, 0.05f);
        }
        
        else if (_inputManager.UpwardsDown)
        {
            _weapon.transform.localScale = new Vector3(0.05f, 0.35f);
            _weapon.transform.localPosition = new Vector3(0f, 0.15f);
        }
    }

    
    
    private void Roll()
    {
        var moveSpeedTemp = _maxMoveSpeed;
        if (_inputManager.IsRolling && IsGrounded)
        {
            _animator.SetBool("IsRolling",true);
            transform.localScale = new Vector3(9f, 9f, 1);
            if (_player.velocity.x > 0 || _player.velocity.x < 0)
            {
                _maxMoveSpeed = Mathf.Lerp(4f, 12f, _rollInterpolator);
                _rollInterpolator += 0.4f * Time.deltaTime;
            }
        }
        
        else if (!_inputManager.HasExitedRoll && !UnderRoof || _isOnWall)
        {
            _animator.SetBool("IsRolling",false);
            transform.localScale = new Vector3(11,11,1);
            _maxMoveSpeed = 12f;
            _rollInterpolator = 0f;
        }
    }

    
    
    private void WallJump(Vector2 dir)
    {
        if (_isOnWall && Input.GetButtonDown("Jump") && _player.velocity.x < 0f || _isOnWall && Input.GetButtonDown("Jump") && 0f < _player.velocity.x)
        {
            _player.velocity = new Vector2((dir.x * 2) * _maxMoveSpeed * 1.5f, JumpForce * 1.5f);
            _hasWallJumped = true;
            _canDoubleJump = true;
            StartCoroutine(WallJumpTimer());
        }
    }

    
    
    private void OnCollisionStay2D(Collision2D col)
    {
        if (Unit.HasWallJump)
        {
            if (col.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                _player.velocity = new Vector2(_player.velocity.x, SlideSpeed * Time.deltaTime);
                _isOnWall = true;
                _animator.SetBool("IsOnWall", true);
            }
        }
    }

    
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Wind") && _isGliding)
            _player.velocity = new Vector2(_player.velocity.x, GlideSpeed * Time.deltaTime);
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

        yield return new WaitForSeconds(0.1f);
            
        _hasWallJumped = false;
    }

    
    
    private void Dash(float x, float y)
    {
        if (_inputManager.DashDown && _canDash)
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


    
    private IEnumerator DashWait()
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
            _player.velocity = new Vector2(0f, -1f);

            if (Input.GetKey(KeyCode.Mouse1))
                _player.gravityScale = 0.4f;
        }

        
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _player.gravityScale = 2.6f;
            _betterJump.enabled = true;
            _isGliding = false;
        }
    }


    private void CreateDust() => _dust.Play();

    
    
    private void CreateDashTrail() => _dashTrail.Play();


    private void ApplyLinearDrag() //NEW
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f)
            _player.drag = _linearDrag;
        else
            _player.drag = 0;
    }


    
    private void ChangeAnimationState(string newState) //NEW
    {
        //stop the same animation from interrupting itself
        if (_currentState == newState) return;
        
        //play the animation
        _animator.Play(newState);
        
        //reassigning the current state
        _currentState = newState;
    }


    
    private void DustOnLand()
    {
        if (!IsGrounded)
        {
            _groundTouched = false;
            _hasCreatedDust = false;
        }
        
        else
            _groundTouched = true;

        if (_groundTouched && !_hasCreatedDust)
        {
            CreateDust();
            _groundTouched = false;
            _hasCreatedDust = true;
        } 
    }
}
