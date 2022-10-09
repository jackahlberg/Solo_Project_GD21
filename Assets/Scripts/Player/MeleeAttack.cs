using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private GameObject _weapon;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private int _damage;
    private HealthManager _health;

    public bool IsAttacking;
    public float SwingTimer;
    public float KnockbackValue;
    public float KnockbackSelf;

    
    private void Update() => Attack();


    private void Attack()
    {
        if (_inputManager.AttackDown && !IsAttacking)
        {
            IsAttacking = true;
            _weapon.SetActive(true);
            StartCoroutine(AttackTimer());
        }
    }


    
    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(SwingTimer);
        IsAttacking = false;
        _weapon.SetActive(false);
    }

    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            var enemy = DealDamage(col);
        }
    }

    
    
    private GameObject DealDamage(Collider2D col)
    {
        var enemy = col.gameObject;
        _health = col.gameObject.GetComponent<HealthManager>();
        _health.DamageHealth(_damage);

        var knockBack = transform.position - col.transform.position;
        col.attachedRigidbody.AddForce(knockBack.normalized * -KnockbackValue);
        _rb.AddForce(knockBack.normalized * KnockbackSelf);
        return enemy;
    }
}
