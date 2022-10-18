using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float HorizontalInput;
    public float VerticalInput;
    public bool IsRolling;
    public bool HasExitedRoll;
    public bool DashDown;
    public bool JumpDown;
    public bool IsJumping;
    public bool AttackDown;
    public bool UpwardsDown;
    public bool DownwardsDown;


    private void Update() => GetInput();

    
    private void GetInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        //VerticalInput = Input.GetAxisRaw("Vertical");
        IsRolling = Input.GetKey(KeyCode.LeftControl);
        HasExitedRoll = Input.GetKeyUp(KeyCode.LeftControl);
        DashDown = Input.GetKeyDown(KeyCode.LeftShift);
        JumpDown = Input.GetButtonDown("Jump");
        IsJumping = Input.GetButton("Jump");
        AttackDown = Input.GetKeyDown(KeyCode.Mouse0);
        UpwardsDown = Input.GetKeyDown(KeyCode.W);
        DownwardsDown = Input.GetKeyDown(KeyCode.S);
    }
}
