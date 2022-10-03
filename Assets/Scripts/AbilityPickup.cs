using System.Runtime.CompilerServices;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    private UnitSO _unit;

    public enum Abilities
    {
        Dash, WallJump, Roll, Glide, DoubleJump
    }

    [SerializeField] private Abilities ability;
    private void Start()
    {
        _unit = GameObject.FindWithTag("Player").GetComponent<OfficialPlayerController>().unit;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            switch (ability)
            {
                case Abilities.Dash:
                    _unit.hasDash = true;
                    
                    break;
                case Abilities.Glide:
                    _unit.hasGlide = true;
                    
                    break;
                case Abilities.Roll:
                    _unit.hasRoll = true;
                    
                    break;
                case Abilities.DoubleJump:
                    _unit.hasDoubleJump = true;
                        
                    break;
                case  Abilities.WallJump:
                    _unit.hasWallJump = true;
                    
                    break;
            }
            
            Destroy(gameObject);
        }
    }
}
