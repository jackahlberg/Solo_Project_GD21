using UnityEngine;
using Vector2 = UnityEngine.Vector2;
public class EnemyFollowPlayer : MonoBehaviour
{

    public float jumpHeight;
    
    public float speed;
    private Transform _player;
    public Rigidbody2D me;
    public float groundCheckDistance;

    private bool _jumpToRight;
    private bool _jumpToLeft;
    public bool _isGrounded;
    
    [SerializeField] private LayerMask groundLayer;
    private EnemyMelee _attack;
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _attack = GetComponent<EnemyMelee>();
    }

    void Update()
    {
        _isGrounded = Physics2D.Raycast(me.position, Vector2.down, groundCheckDistance, groundLayer);
        _jumpToRight = Physics2D.Raycast(me.transform.position, Vector2.right, 1.5f, groundLayer);
        _jumpToLeft = Physics2D.Raycast(me.transform.position, Vector2.left, 1.5f, groundLayer);

        var playerPos = transform.position.x - _player.transform.position.x;
        
        if (playerPos < 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }

        if (playerPos > 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        
        MoveTowards();
        Jump();
    }

    private void MoveTowards()
    {
        if (Vector2.Distance(me.transform.position, _player.transform.position) < 3f || Vector2.Distance(me.transform.position, _player.transform.position) > 20f)
        {
            _attack.Attack();
            enabled = false;
        }
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, speed * Time.deltaTime);
    }

    private void Jump()
    {
        if (_jumpToLeft && _isGrounded)
        {
            me.velocity = new Vector2(me.velocity.x, jumpHeight);
        }
        else if (_jumpToRight && _isGrounded)
        {
            me.velocity = new Vector2(me.velocity.x, jumpHeight);
        }
        
    }
}
