using System.Collections;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{

    [SerializeField] private GameObject _weapon;
    [SerializeField] private EnemyFollowPlayer _enemyFollowPlayer;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public float SwingTimer = 0.5f;
    public float AnticipationTime = 1;
    public int Damage;

    
    public void Attack() => StartCoroutine(SwingTimerWait());

    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var playerHealth = col.gameObject.GetComponent<HealthManager>();
            playerHealth.DamageHealth(Damage);
            
            StartCoroutine(DamagedSlowDown());
        }
    }
    
    
    
    private IEnumerator SwingTimerWait()
    {
        _spriteRenderer.color= Color.black;
        
        yield return new WaitForSeconds(AnticipationTime);
        _spriteRenderer.color = Color.white;
        _weapon.SetActive(true);
        
        yield return new WaitForSeconds(SwingTimer);
        _weapon.SetActive(false);
        _enemyFollowPlayer.enabled = true;
        enabled = false;
    }
    
    
    
    public IEnumerator DamagedSlowDown()
    {
        Time.timeScale = 0.001f;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
    }
}
