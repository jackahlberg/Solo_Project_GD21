using UnityEngine;

public class DeathOnCollision : MonoBehaviour
{

    public Vector3 RespawnPoint;

    private void Start() => RespawnPoint = transform.position;

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Checkpoint"))
            RespawnPoint = transform.position;
        
        else if (col.CompareTag("Spike"))
            transform.position = RespawnPoint;
    }
}
