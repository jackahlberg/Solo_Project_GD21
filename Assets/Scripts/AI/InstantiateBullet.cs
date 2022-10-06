using UnityEngine;

public class InstantiateBullet : MonoBehaviour
{
    private Transform _enemyTransform;
    [SerializeField] private GameObject _bulletPrefab;

    private void Start()
    {
        _enemyTransform = GetComponent<Transform>();
        InvokeRepeating("Instantiate", 1.5f, 2.5f);
    }

    
    private void Instantiate()
    {
        var enemyPos = _enemyTransform.position;
        var position = new Vector2(enemyPos.x, enemyPos.y);
        Instantiate(_bulletPrefab, position, Quaternion.identity);
    }
}
