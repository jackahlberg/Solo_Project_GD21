/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumpErick : MonoBehaviour
{

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D _player;
    private InputManagerErick _inputManager;

    private void Start()
    {
        _player = GetComponent<Rigidbody2D>();
        _inputManager = gameObject.GetComponent<InputManagerErick>();
    }

    private void Update()
    {
        JumpGravity();
    }

    private void JumpGravity()
    {
        //NEW
        if (_player.velocity.y < 0)
        {
            _player.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_player.velocity.y > 0 && !_inputManager.jumpingInput)
        {
            _player.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}*/
