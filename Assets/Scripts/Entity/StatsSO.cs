using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat SO", menuName = "Stat")]
public class StatsSO : ScriptableObject
{
    public string name;
    public float movementSpeed;
    public float jumpForce;
    public float doubleJumpForce;
    public float dashMultiplier;
}
