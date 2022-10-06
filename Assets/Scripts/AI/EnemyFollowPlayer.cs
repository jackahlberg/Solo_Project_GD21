using UnityEngine;
using Vector2 = UnityEngine.Vector2;
public class EnemyFollowPlayer : MonoBehaviour
{

    public float JumpHeight;
    
    public float speed;
    private Transform _player;
    public Rigidbody2D _rb;
    public float groundCheckDistance;

    private bool _jumpToRight;
    private bool _jumpToLeft;
    public bool _isGrounded;
    
    [SerializeField] private LayerMask groundLayer;
    private EnemyMelee _attack;

    
    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _attack = GetComponent<EnemyMelee>();
        _rb = GetComponent<Rigidbody2D>();
    }

    
    
    private void Update()
    {
        _isGrounded = Physics2D.Raycast(_rb.position, Vector2.down, groundCheckDistance, groundLayer);
        _jumpToRight = Physics2D.Raycast(_rb.transform.position, Vector2.right, 1.5f, groundLayer);
        _jumpToLeft = Physics2D.Raycast(_rb.transform.position, Vector2.left, 1.5f, groundLayer);

        var playerPos = transform.position.x - _player.transform.position.x;
        
        if (playerPos < 0)
            transform.localScale = new Vector2(1, transform.localScale.y);
        

        if (playerPos > 0)
            transform.localScale = new Vector2(-1, transform.localScale.y);

        MoveTowards();
        Jump();
    }

    
    
    private void MoveTowards()
    {
        if (Vector2.Distance(_rb.transform.position, _player.transform.position) < 3f || Vector2.Distance(_rb.transform.position, _player.transform.position) > 20f)
        {
            _attack.Attack();
            enabled = false;
        }
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, speed * Time.deltaTime);
    }

    
    
    private void Jump()
    {
        if (_jumpToLeft && _isGrounded)
            _rb.velocity = new Vector2(_rb.velocity.x, JumpHeight);
        
        
        else if (_jumpToRight && _isGrounded)
            _rb.velocity = new Vector2(_rb.velocity.x, JumpHeight);
    }
}
