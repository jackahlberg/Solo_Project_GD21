using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class UnitSO : ScriptableObject
{ 
        public new string name;
        public bool hasWalk;
        public bool hasDash;
        public bool hasJump;
        public bool hasGlide;
        public bool hasWallJump;
        public bool hasRoll;
}
