using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float walkInput;
    public bool rollInput;
    public bool exitRollInput;
    public bool dashInput;
    public bool jumpInput;
    public bool jumpingInput;
    public bool attackInput;
    public bool upInput;
    public bool downInput;


    private void Update() => GetInput();

    
    private void GetInput()
    {
        walkInput = Input.GetAxisRaw("Horizontal");
        rollInput = Input.GetKey(KeyCode.LeftControl);
        exitRollInput = Input.GetKey(KeyCode.LeftControl);
        dashInput = Input.GetKeyDown(KeyCode.LeftShift);
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        jumpingInput = Input.GetButton("Jump");
        attackInput = Input.GetKeyDown(KeyCode.Mouse0);
        upInput = Input.GetKeyDown(KeyCode.W);
        downInput = Input.GetKeyDown(KeyCode.S);
    }
}
