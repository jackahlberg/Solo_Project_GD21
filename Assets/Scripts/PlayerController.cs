using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    public float doubleJumpSpeed = 4f;
    private float direction = 0f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isGrounded;

    public int jumpCount;
    private bool facingRight;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        jumpCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");
        player.velocity = new Vector2(direction * speed, player.velocity.y);

        if (direction > 0f && facingRight)
        {
            Flip();
        }
        else if (direction < 0f && !facingRight)
        {
            Flip();
        }

        Jumping();
    }

    private void Jumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && jumpCount == 0)
        {
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
            jumpCount += 1;
        }
        //Double Jump
        else if (Input.GetButtonDown("Jump") && jumpCount == 1)
        {
            player.velocity = new Vector2(player.velocity.x, doubleJumpSpeed);
            jumpCount = 0;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180f, 0);
    }
}