using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int Health { get; private set; }

    [SerializeField] private float cameraShakeIntensity;
    [SerializeField] private float cameraShakeDuration;

    [SerializeField] private float invulnerableDuration;
    private bool _isInvulnerable;
    
    [SerializeField] private GameObject coins;

    private void Start()
    {
        Health = maxHealth;
    }

    private void Update()
    {

    }

    public void UpdateHealth(int damage)
    {
        if (!_isInvulnerable)
        {
            Health -= damage;
            VirtualMachineShake.Instance.CameraShake(cameraShakeIntensity, cameraShakeDuration);
            Debug.Log(gameObject.name + Health);
            CheckHealth();

            if (gameObject.CompareTag("Player"))
            {
                StartCoroutine(InvulnerabilityTimer());
            }
        }
    }

    IEnumerator InvulnerabilityTimer()
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(1);
        _isInvulnerable = false;
    }

    private void CheckHealth()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (Health <= 0)
            {
                LoadScene.Respawn();
            }
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            if (Health <= 0)
            {
                var amount = Random.Range(1, 7);

                for (int i = 0; i < amount; i++)
                {
                    var direction = Random.Range((float) -120, 120);
                    var force = Random.Range(100, 300);
                    var spawnedGold = Instantiate(coins, transform.position, Quaternion.identity);
                    spawnedGold.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, force));
                }
                
                Destroy(gameObject);
            }
        }
        
    }
}


