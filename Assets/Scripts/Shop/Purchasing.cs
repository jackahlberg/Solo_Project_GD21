using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchasing : MonoBehaviour
{

    [Header("Purchase Info")] [SerializeField]
    private int price;
    
    //Hidden Variables

    private CurrencyManager _playerMoney;
    private ShopUI _ui;
    // Start is called before the first frame update
    void Start()
    {
        _ui = FindObjectOfType<ShopUI>();
        _playerMoney = GameObject.FindWithTag("Player").GetComponent<CurrencyManager>();
    }

    public void Buy()
    {
        if (_playerMoney.Coins >= price)
        {
            _playerMoney.Coins -= price;
        }
        else
        {
            _ui.NotEnoughMoney();
        }
    }
}
