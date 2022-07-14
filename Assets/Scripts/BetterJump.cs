using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{

    public float fallMultiplier = 2.5f;
    public float lowJumpMulitplier = 2f;

    private Rigidbody2D _player;

    private void Start()
    {
        _player = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        JumpGravity();
    }

    private void JumpGravity()
    {
        if (_player.velocity.y < 0)
        {
            _player.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_player.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _player.velocity += Vector2.up * Physics.gravity.y * (lowJumpMulitplier - 1) * Time.deltaTime;
        }
        
    }
}
