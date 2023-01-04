using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchasing : MonoBehaviour
{

    [Header("Purchase Info")]
    [SerializeField] private int price;
    private enum Type
    {
        Weapon, StatUpgrade, Health, Ability
    }

    private enum AbilityType
    {
        Dash, DoubleJump, Glide, WallJump, Roll
    }

    [SerializeField] private Type type;

    [Header("If healing")]
    [SerializeField] private int healAmount;

    [Header("If Ability")] 
    [SerializeField] private AbilityType abilityType;
    
    //Hidden Variables

    private CurrencyManager _playerMoney;
    private HealthManager _playerHealth;
    private ShopUI _ui;
    private UnitSO Unit;

    void Start()
    {
        _ui = FindObjectOfType<ShopUI>();
        _playerMoney = GameObject.FindWithTag("Player").GetComponent<CurrencyManager>();
        _playerHealth = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
        Unit = GameObject.FindWithTag("Player").GetComponent<OfficialPlayerController>().Unit;
    }

    public void Buy()
    {
        //Reducts money from the player
        
        if (_playerMoney.Coins >= price)
        {
            _playerMoney.Coins -= price;
        }
        else
        {
            _ui.NotEnoughMoney();
        }
        
        switch (type)
        {
            case Type.Health:
                _playerHealth.RegainHealth(healAmount);
                break;
            
            case Type.Ability:
                
                //Checks what ability type should be added (selected in inspector)
                //Then switches that bool in the SO to true
                
                switch (abilityType)
                {
                    case AbilityType.Dash:
                        Unit.HasDash = true;
                        break;
                    
                    case AbilityType.Glide:
                        Unit.HasGlide = true;
                        break;
                    
                    case AbilityType.Roll:
                        Unit.HasRoll = true;
                        break;
                    
                    case AbilityType.DoubleJump:
                        Unit.HasDoubleJump = true;
                        break;
                    
                    case AbilityType.WallJump:
                        Unit.HasWallJump = true;
                        break;
                }
            break;
                
            case Type.StatUpgrade:
                //Check if a stat upgrade has reached max possible. If so, regain the money spent and print something on screen. 
                //More preferably, do not show this option at all. Make the check when the shop is being opened through ShopUI script
                //Increase a stat using method, will likely need an additional Enum to decide which one
            
            case Type.Weapon:
                //If weapons can be bought, spawn the weapon to the player inventory here.
            break;
        }

    }

}
