using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public float speed = 5f;
    public float jumpSpeed = 8f;
    public float doubleJumpSpeed = 4f;
    private int jumpCount;
    private bool facingRight;
    private float direction = 0f;
    
    private Rigidbody2D player;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D crouchCollider;
    private CapsuleCollider2D mainCollider;
    [SerializeField] private Sprite rollSprite;
    [SerializeField] private Sprite mainSprite;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public LayerMask roofLayer;
    public float roofCheckRadius;
    public bool isGrounded;
    public bool underRoof;
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        crouchCollider = GetComponent<CircleCollider2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        jumpCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        underRoof = Physics2D.Raycast(groundCheck.position, Vector2.up, 3f, roofLayer);
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
        Roll();
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

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            spriteRenderer.sprite = rollSprite;
            crouchCollider.enabled = true;
            mainCollider.enabled = false;
            speed = 3f;
        }
        else if (!Input.GetKey(KeyCode.LeftControl) && !underRoof)
        {
            spriteRenderer.sprite = mainSprite;
            crouchCollider.enabled = false;
            mainCollider.enabled = true;
            speed = 5f;
        }
    }
}