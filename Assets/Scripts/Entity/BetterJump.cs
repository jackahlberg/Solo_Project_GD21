using UnityEngine;

public class BetterJump : MonoBehaviour
{

    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;
    private Rigidbody2D _rb;
    private InputManager _inputManager;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputManager = GetComponent<InputManager>();
    }



    private void Update() => JumpGravity();



    private void JumpGravity()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
            Debug.Log("Fall mulitplier");

        }
        else if (_rb.velocity.y > 0 && !_inputManager.IsJumping)
        {   
            _rb.velocity += Vector2.up * Physics.gravity.y * (LowJumpMultiplier - 1) * Time.deltaTime;
            Debug.Log("Low Jump");
            
        }

    }
}
