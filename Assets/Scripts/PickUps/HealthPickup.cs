using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private int _healAmount;

    private enum HealType
    {
        SmallHeal, MediumHeal, LargeHeal
    }

    [SerializeField] private HealType _healType;

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            switch (_healType)
            {
                case HealType.SmallHeal:
                    _healAmount = 1;
                    break;
                
                case HealType.MediumHeal:
                    _healAmount = 3;
                    break;
                
                case HealType.LargeHeal:
                    _healAmount = 5;
                    break;
            }

            var healthManager = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
            healthManager.RegainHealth(_healAmount);
            
            Destroy(gameObject);
        }
    }
}
