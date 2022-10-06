
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float Speed;
    private Transform Target;
    
    private void Start() => Target = GameObject.FindWithTag("Player").transform;


    private void Update() => transform.position = Vector3.MoveTowards
        (transform.position, Target.transform.position, Speed * Time.deltaTime);
}
