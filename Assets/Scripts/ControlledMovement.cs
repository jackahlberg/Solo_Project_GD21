using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledMovement : MonoBehaviour
{
    //References
    private InputManager _inputManager;
    private Rigidbody2D _player;

    //Speed And JumpForce
    public float speed = 8f;
    public float jumpForce = 8f;
    
    //Surface Checks
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isGrounded;
    public float _groundedRememberTime;
    private float _coyoteTime;
    public float _jumpRememberTime;
    private float _jumpRemember;
    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var x = _inputManager.walkInput;
        var y = _inputManager.jumpInput;
        var dir = new Vector2(x, y);
        
        SurfaceChecks();
        Walk(dir);
        Jump();
    }
    
    private void SurfaceChecks()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        _coyoteTime -= Time.deltaTime;
        if (isGrounded)
        {
            _coyoteTime = _groundedRememberTime;
        }

        _jumpRemember -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            _jumpRemember = _jumpRememberTime;
        }
    }
    
    private void Walk(Vector2 dir)
    {
        _player.velocity = new Vector2(dir.x * speed, _player.velocity.y);
    }
    
    private void Jump()
    {
        if ((_coyoteTime > 0) && (_jumpRemember > 0))
        {
            _player.velocity = new Vector2(_player.velocity.x, jumpForce);
        }

    }
}
