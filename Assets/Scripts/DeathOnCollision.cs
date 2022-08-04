using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathOnCollision : MonoBehaviour
{

    public Vector3 respawnPoint;

    private void Start()
    {
        respawnPoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Checkpoint"))
        {
            respawnPoint = transform.position;
        }
        else if (col.CompareTag("Spike"))
        {
            transform.position = respawnPoint;
        }
    }
}
