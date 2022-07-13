using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float walkInput;
    public bool rollhInput;
    public bool exitRollInput;
    public bool jumpInput;
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        walkInput = Input.GetAxis("Horizontal");
        rollhInput = Input.GetKeyDown(KeyCode.LeftControl);
        exitRollInput = Input.GetKey(KeyCode.LeftControl);
        jumpInput = Input.GetButtonDown("Jump");
    }
}
