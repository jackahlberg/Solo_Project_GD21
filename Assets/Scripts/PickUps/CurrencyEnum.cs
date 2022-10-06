using UnityEngine;

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

    [SerializeField] private CurrencyAmount _currencyAmount;
    [SerializeField] private CurrencyType _currencyType;


    private void Start() => _playerCurrency = GameObject.FindWithTag("Player").GetComponent<CurrencyContainer>();

    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            switch (_currencyAmount)
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

            switch (_currencyType)
            {
                case CurrencyType.Coin:
                    break;
            
                case CurrencyType.Diamond:
                    break;
            }
            
            AdjustCurrencyAmount(_amount);
        
            Destroy(gameObject);
        }
    }
    
    
    
    public void AdjustCurrencyAmount(int amount)
    {
        if (_currencyType == CurrencyType.Coin)
            _playerCurrency.Coins += amount;
        
        else if (_currencyType == CurrencyType.Diamond)
            _playerCurrency.Diamonds += amount;
    }
}
