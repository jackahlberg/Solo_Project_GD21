using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopUI : MonoBehaviour
{

    [Header("References")]

    [SerializeField] private GameObject notEnoughMoney;
    [SerializeField] private GameObject shopInterface;
    
    //Hidden Variables
    private bool _inRange;
    private bool _inShop;
    private OfficialPlayerController _playerController;
    private MeleeAttack _attack;
    
    void Start()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<OfficialPlayerController>();
        _attack = GameObject.FindWithTag("Player").GetComponent<MeleeAttack>();
    }

    private void Update()
    {
        if (_inRange && Input.GetKeyDown(KeyCode.E) && !_inShop)
        {
            _inShop = true;
            shopInterface.SetActive(true);
            _playerController.enabled = false;
            _attack.enabled = false;
        }
        else if (_inShop && Input.GetKeyDown(KeyCode.E))
        {
            _inShop = false;
            shopInterface.SetActive(false);
            _playerController.enabled = true;
            _attack.enabled = true;
        }
    }

    public void NotEnoughMoney()
    {
        if (!notEnoughMoney.activeInHierarchy)
        {
            notEnoughMoney.SetActive(true);
            StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        notEnoughMoney.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _inRange = false;
    }
}
