using UnityEngine;

public class InputManagerErick : MonoBehaviour
{
    public float walkInput;
    public bool rollhInput;
    public bool exitRollInput;
    public bool dashInput;
    public bool jumpInput;
    public bool attackInput;
    public bool upInput;
    public bool downInput;

    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        walkInput = Input.GetAxisRaw("Horizontal");
        rollhInput = Input.GetKey(KeyCode.LeftControl);
        exitRollInput = Input.GetKey(KeyCode.LeftControl);
        dashInput = Input.GetKeyDown(KeyCode.LeftShift);
        jumpInput = Input.GetButton("Jump"); //NEW
        attackInput = Input.GetKeyDown(KeyCode.Mouse0);
        upInput = Input.GetKeyDown(KeyCode.W);
        downInput = Input.GetKeyDown(KeyCode.S);
    }
}
