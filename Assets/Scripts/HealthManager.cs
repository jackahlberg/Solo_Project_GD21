using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int Health { get; private set; }

    [SerializeField] private float cameraShakeIntensity;
    [SerializeField] private float cameraShakeDuration;

    private void Start()
    {
        Health = maxHealth;
    }

    public void UpdateHealth(int damage)
    {
        Health -= damage;
        VirtualMachineShake.Instance.CameraShake(cameraShakeIntensity, cameraShakeDuration);
        Debug.Log(gameObject.name + Health);
    }
}


