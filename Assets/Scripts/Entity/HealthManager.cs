using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthManager : MonoBehaviour
{
    [Header("HEALTH")]
    [SerializeField] private int _maxHealth;
    public int _health { get; private set; }

    [Header("HIT FX")]
    [SerializeField] private float _cameraShakeIntensity;
    [SerializeField] private float _cameraShakeDuration;
    [SerializeField] private float _invulnerableDuration;
    
    [Header("PICKUP DROP PREFABS")]
    [SerializeField] private GameObject _coins;
    
    private bool _isInvulnerable;
    private SceneChanger _sceneChanger;

    
    private void Awake() => _sceneChanger = FindObjectOfType<SceneChanger>();

    
    private void Start() => _health = _maxHealth;


    public void DamageHealth(int damage)
    {
        if (!_isInvulnerable)
        {
            _health -= damage;
            VirtualMachineShake.Instance.CameraShake(_cameraShakeIntensity, _cameraShakeDuration);
            CheckHealth();

            if (gameObject.CompareTag("Player"))
                StartCoroutine(InvulnerabilityTimer());
        }
    }

    
    
    public void RegainHealth(int healAmount)
    {
        _health += healAmount;
        CheckHealth();
    }


    
    private IEnumerator InvulnerabilityTimer()
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(_invulnerableDuration);
        _isInvulnerable = false;
    }

    private void CheckHealth()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (_health > _maxHealth)
                _health = _maxHealth;

            if (_health <= 0)
                _sceneChanger.Respawn();

            Debug.Log(_health);
        }
        
        
        else if (gameObject.CompareTag("Enemy"))
        {
            if (_health <= 0)
            {
                var amount = Random.Range(1, 7);

                for (int i = 0; i < amount; i++)
                {
                    var direction = Random.Range((float) -120, 120);
                    var force = Random.Range(100, 300);
                    var spawnedGold = Instantiate(_coins, transform.position, Quaternion.identity);
                    spawnedGold.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, force));
                }
                Destroy(gameObject);
            }
        }
    }
}


