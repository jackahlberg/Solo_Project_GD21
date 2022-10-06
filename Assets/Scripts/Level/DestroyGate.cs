using UnityEngine;

public class DestroyGate : MonoBehaviour
{
    public GameObject[] Gates;
    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Key"))
        {
            for (int i = 0; i < Gates.Length; i++)
            {
                Destroy(Gates[i]);
                Destroy(gameObject);
            }
        }
    }
}
