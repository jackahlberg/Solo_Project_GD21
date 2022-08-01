using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathOnCollision : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SceneManager.LoadScene("Platforming_Level");
        }
    }
}
