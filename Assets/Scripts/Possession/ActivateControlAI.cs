using UnityEngine;
using Cinemachine;

public class ActivateControlAI : MonoBehaviour
{
    [SerializeField] private OfficialPlayerController playerController;
    [SerializeField] private Transform _playerPos;
    [SerializeField] private OfficialPlayerController _controlledEnemy;
    [SerializeField] private Transform _enemyPos;
    [SerializeField] private CinemachineVirtualCamera _cineCam;
    private Rigidbody2D _enemyRb;
    private Rigidbody2D _playerRb;

    private bool _inRange;
    private bool _isControlling;

    private void Start()
    {
        _enemyRb = _enemyPos.gameObject.GetComponent<Rigidbody2D>();
        _playerRb = _playerPos.gameObject.GetComponent<Rigidbody2D>();
    }

    
    
    private void Update()
    {
        if (_inRange && Input.GetKeyDown(KeyCode.E) && !_isControlling)
        {
            _isControlling = true;
            playerController.enabled = false;
            _controlledEnemy.enabled = true;
            _playerRb.velocity = Vector3.zero;
        }
        
        else if (Input.GetKeyDown(KeyCode.E) && _isControlling)
        {
            _isControlling = false;
            playerController.enabled = true;
            _controlledEnemy.enabled = false;
            _enemyRb.velocity = Vector3.zero;
            _cineCam.Follow = _playerPos;
        }
    }

    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _inRange = true;
            _cineCam.Follow = _enemyPos;
        }
    }

    
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _cineCam.Follow = _playerPos;
            _inRange = false;
        }
    }
}
