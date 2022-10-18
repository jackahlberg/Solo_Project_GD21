using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _coinsText;
    private HealthManager _playerHealth;
    private CurrencyManager _playerCurrency;

    
    private void Start()
    {
        _playerHealth = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
        _playerCurrency = GameObject.FindWithTag("Player").GetComponent<CurrencyManager>();
    }

    
    
    private void Update()
    {
        DisplayHealth();
        DisplayCurrency();
    }

    
    
    private void DisplayHealth() => _healthText.SetText("Health: " + _playerHealth._health);
    

    private void DisplayCurrency() => _coinsText.SetText("Coins: " + _playerCurrency.Coins);
}
