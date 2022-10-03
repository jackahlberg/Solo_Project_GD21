using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThroughSide : MonoBehaviour
{
    private OfficialPlayerController _player;
    private Rigidbody2D _playerRigidbody;
    private BoxCollider2D _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _player = GameObject.FindWithTag("Player").GetComponent<OfficialPlayerController>();
        _playerRigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_player.isDashing && _playerRigidbody.velocity.x > 0 || _player.isDashing && _playerRigidbody.velocity.x < 0)
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
