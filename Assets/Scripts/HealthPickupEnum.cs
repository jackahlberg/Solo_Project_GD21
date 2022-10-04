using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupEnum : MonoBehaviour
{

    private int _healAmount;
    enum HealthPickup
    {
        SmallHealth, MediumHealth, LargeHealth
    }

    [SerializeField] private HealthPickup healType;

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            switch (healType)
            {
                case HealthPickup.SmallHealth:
                    _healAmount = 1;
                    break;
                case HealthPickup.MediumHealth:
                    _healAmount = 3;
                    break;
                case HealthPickup.LargeHealth:
                    _healAmount = 5;
                    break;
            }

            HealthManager healthManager = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
            
            healthManager.RegainHealth(_healAmount);
        }
    }
}
