using System.Collections;
using UnityEngine;

public class PickupAttractor : MonoBehaviour
{
    public float speed;
    private bool _colliderTrigger;


    private void Awake() => StartCoroutine(CollisionTimer());
    
    private void Start() => _colliderTrigger = GetComponent<CircleCollider2D>().isTrigger;

    
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, other.gameObject.transform.position, speed * Time.deltaTime);
        }
    }


    private IEnumerator CollisionTimer()
    {
        _colliderTrigger = true;

        yield return new WaitForSeconds(0.2f);

        _colliderTrigger = false;
    }
}
