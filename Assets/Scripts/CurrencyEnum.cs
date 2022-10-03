using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class CurrencyEnum : MonoBehaviour
{

    private CurrencyContainer _playerCurrency;
    private int _amount;
    private enum CurrencyAmount
    {
        Coin, BigCoin, Diamond, BigDiamond
    }
    
    public enum CurrencyType
    {
        Coin, Diamond
    }

    [SerializeField] private CurrencyAmount currencyAmount;
    [SerializeField] private CurrencyType currencyType;

    public void OnTriggerEnter2D(Collider2D col)
    {
        switch (currencyAmount)
        {
            case CurrencyAmount.Coin:
                _amount = 1;
                break;
            case CurrencyAmount.Diamond:
                _amount = 1;
                break;
            case CurrencyAmount.BigCoin:
                _amount = 5;
                break;
            case CurrencyAmount.BigDiamond:
                _amount = 3;
                break;
        }

        switch (currencyType)
        {
            case CurrencyType.Coin:
                break;
            
            case CurrencyType.Diamond:
                break;
        }
        
        AdjustCurrencyAmount(_amount);
        
        Destroy(gameObject);
    }
    
    public void AdjustCurrencyAmount(int amount)
    {
        if (currencyType == CurrencyType.Coin)
        {
            _playerCurrency.coins += amount;
        }
        else if (currencyType == CurrencyType.Diamond)
        {
            _playerCurrency.diamonds += amount;
        }
    }

    void Start()
    {
        _playerCurrency = GameObject.FindWithTag("Player").GetComponent<CurrencyContainer>();
    }


}
