using System;
using Unity.VisualScripting;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int Coins;
    public int Diamonds;

    private void Start()
    {
        Coins = 100;
    }
}
