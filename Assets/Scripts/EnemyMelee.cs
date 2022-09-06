using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{

    [SerializeField] private GameObject weapon;
    [SerializeField] private EnemyFollowPlayer follow;
    [SerializeField] private SpriteRenderer color;
    public float swingTimer = 0.5f;
    public float anticipationTime = 1;

    public void Attack()
    {
        StartCoroutine(SwingTimer());
    }

    IEnumerator SwingTimer()
    {
        color.color= Color.black;
        yield return new WaitForSeconds(anticipationTime);
        color.color = Color.white;
        weapon.SetActive(true);
        yield return new WaitForSeconds(swingTimer);
        weapon.SetActive(false);
        follow.enabled = true;
        enabled = false;
    }
}
