using System.Collections;
using UnityEngine;

public class DeathByBullet : MonoBehaviour
{
    private GameObject _bullet;
    private DeathOnCollision _checkpoint;

    
    private void Start() => _checkpoint = GetComponent<DeathOnCollision>();
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (CompareTag("Player") && col.CompareTag("Bullet") || CompareTag("Player") && col.CompareTag("Key"))
        {
            _bullet = col.gameObject;
            StartCoroutine(DeflectCheck());
        }
        
        else if (CompareTag("Enemy") && col.CompareTag("Bullet") || CompareTag("Enemy") && col.CompareTag("Key"))
            Destroy(gameObject);
    }


    private IEnumerator DeflectCheck()
    {
        yield return new WaitForSeconds(0.25f);
        
        if (Vector2.Distance(_bullet.transform.position, transform.position) < 1.5f)
        {
            transform.position = _checkpoint.RespawnPoint;
            Destroy(_bullet);
        }
    }
}
