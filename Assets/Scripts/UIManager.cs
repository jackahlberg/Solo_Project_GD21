using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text coinsText;
    private HealthManager _playerHealth;
    private CurrencyContainer _playerCurrency;

    private void Start()
    {
        _playerHealth = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
        _playerCurrency = GameObject.FindWithTag("Player").GetComponent<CurrencyContainer>();
    }

    private void Update()
    {
        DisplayHealth();
        DisplayCurrency();
    }

    private void DisplayHealth()
    {
        healthText.SetText("Health: " + _playerHealth.Health);
    }

    private void DisplayCurrency()
    {
        coinsText.SetText("Coins: " + _playerCurrency.coins);
    }
}
