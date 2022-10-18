using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    private UnitSO _unit;

    public enum Abilities
    {
        Dash, WallJump, Roll, Glide, DoubleJump
    }

    [SerializeField] private Abilities ability;
    
    
    private void Start() => 
        _unit = GameObject.FindWithTag("Player").GetComponent<OfficialPlayerController>().Unit;

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            switch (ability)
            {
                case Abilities.Dash:
                    _unit.HasDash = true;
                    break;
                
                case Abilities.Glide:
                    _unit.HasGlide = true;
                    break;
                
                case Abilities.Roll:
                    _unit.HasRoll = true;
                    break;
                
                case Abilities.DoubleJump:
                    _unit.HasDoubleJump = true;
                    break;
                
                case  Abilities.WallJump:
                    _unit.HasWallJump = true;
                    break;
            }
            
            Destroy(gameObject);
        }
    }
}
