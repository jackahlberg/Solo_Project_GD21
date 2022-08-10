using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float walkInput;
    public bool rollhInput;
    public bool exitRollInput;
    public bool dashInput;
    public float jumpInput;
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
        jumpInput = Input.GetAxis("Vertical");
    }
}
