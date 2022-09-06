using System.Collections;
using UnityEngine;

public class DashThroughDown : MonoBehaviour
{
    private PlayerController _player;
    private Rigidbody2D _playerRigidbody;
    private BoxCollider2D _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _playerRigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_player.isDashing && _playerRigidbody.velocity.y < 0 || _playerRigidbody.velocity.y > 0 && _player.isDashing)
        {
            StartCoroutine(ColliderEnabled());
        }
    }

    IEnumerator ColliderEnabled()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        _collider.enabled = true;
    }
}
