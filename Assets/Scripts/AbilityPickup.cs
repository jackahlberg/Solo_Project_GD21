using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    [SerializeField] private bool activateDash;
    [SerializeField] private bool activateWallJump;
    [SerializeField] private bool activateRoll;
    [SerializeField] private bool activateGlide;
    [SerializeField] private bool activateDoubleJump;
    private UnitSO _unit;

    private void Start()
    {
        _unit = GameObject.FindWithTag("Player").GetComponent<OfficialPlayerController>().unit;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (activateDash)
            {
                _unit.hasDash = true;
            }
            else if (activateWallJump)
            {
                _unit.hasWallJump = true;
            }
            else if (activateRoll)
            {
                _unit.hasRoll = true;
            }
            else if (activateGlide)
            {
                _unit.hasGlide = true;
            }
            else if (activateDoubleJump)
            {
                _unit.hasDoubleJump = true;
            }
            
            Destroy(gameObject);
        }
    }
}
