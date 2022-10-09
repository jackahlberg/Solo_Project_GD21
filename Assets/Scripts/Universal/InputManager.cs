using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float HorizontalInput;
    public float VerticalInput;
    public bool rollInput;
    public bool exitRollInput;
    public bool dashInput;
    public bool jumpDownInput;
    public bool isJumping;
    public bool attackInput;
    public bool upInput;
    public bool downInput;


    private void Update() => GetInput();

    
    private void GetInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
        rollInput = Input.GetKey(KeyCode.LeftControl);
        exitRollInput = Input.GetKey(KeyCode.LeftControl);
        dashInput = Input.GetKeyDown(KeyCode.LeftShift);
        jumpDownInput = Input.GetButtonDown("Jump");
        isJumping = Input.GetButton("Jump");
        attackInput = Input.GetKeyDown(KeyCode.Mouse0);
        upInput = Input.GetKeyDown(KeyCode.W);
        downInput = Input.GetKeyDown(KeyCode.S);
    }
}
