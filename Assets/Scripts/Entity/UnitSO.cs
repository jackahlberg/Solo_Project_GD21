using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class UnitSO : ScriptableObject
{ 
        public new string Name;
        public bool HasWalk;
        public bool HasDash;
        public bool HasJump;
        public bool HasDoubleJump;
        public bool HasGlide;
        public bool HasWallJump;
        public bool HasRoll;
}
