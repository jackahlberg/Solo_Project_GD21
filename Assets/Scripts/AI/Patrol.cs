using UnityEngine;

public class Patrol : MonoBehaviour
{
    private bool _patrol;
    public float MoveSpeed;
    [SerializeField] private Transform[] _patrolPoints;
    private Transform _myScale;
    private Transform _player;
    private EnemyFollowPlayer _aggro;
    public int i;
    private Rigidbody2D _rb;

    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _aggro = GetComponent<EnemyFollowPlayer>();
        _myScale = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
    }

    
    
    private void Update()
    {
        if (Vector2.Distance(_player.transform.position, transform.position) < 5)
        {
            enabled = false;
            _aggro.enabled = true;
        }
        
        Move();
    }

    
    
    private void FixedUpdate() => _rb.velocity = new Vector2(MoveSpeed * Time.fixedDeltaTime, _rb.velocity.y);

    
    
    private void Move()
    {
        if (Vector2.Distance(transform.position, _patrolPoints[0].transform.position) < 0.5f && _myScale.localScale.x == 1)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            MoveSpeed *= -1;
            i = 1;
        }
        
        if (Vector2.Distance(transform.position, _patrolPoints[1].position) < 0.5f && _myScale.localScale.x == -1)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            MoveSpeed *= -1;
            i = 0;
        }
    }
}
