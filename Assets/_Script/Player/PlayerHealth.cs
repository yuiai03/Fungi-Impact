using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float countChangeDamageSlider;
    [SerializeField] private int takingDamage;
    private PlayerInfoReader playerInfo;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerInfo = GetComponent<PlayerInfoReader>();

        EventManager.onSwitchFungus += OnSwitchFungus;
    }
    private void OnDestroy()
    {
        EventManager.onSwitchFungus -= OnSwitchFungus;
    }
    private void Update()
    {
        UpdateCurrentDamageSlider();
    }
    void OnSwitchFungus(FungusData fungusData)
    {
        playerInfo.playerCurrentHUD.SetCurrentHealthSliderInit(0, fungusData.maxHealth);
        playerInfo.playerCurrentHUD.SetCurrentDamageSliderInit(0, fungusData.maxHealth);
        playerInfo.playerCurrentHUD.SetCurrentHealthSlider(fungusData.health);
        playerInfo.playerCurrentHUD.SetCurrentDamageSlider(fungusData.health);
        playerInfo.playerCurrentHUD.SetHealthText(fungusData.health, fungusData.maxHealth);
    }
    public void TakeDamage(int value)
    {
        playerInfo.PlayerData.health -= value;
        playerInfo.playerCurrentHUD.SetCurrentHealthSlider(playerInfo.PlayerData.health);
        playerInfo.playerCurrentHUD.SetHealthText(playerInfo.PlayerData.health, playerInfo.PlayerData.maxHealth);
        takingDamage = value;

        if (playerInfo.PlayerData.health <= 0)
        {
            FungusDie();         
        }
    }
    void UpdateCurrentDamageSlider()
    {
        float healthValue = playerInfo.playerCurrentHUD.currentHealthSlider.value;
        float damageValue = playerInfo.playerCurrentHUD.currentDamageSlider.value;


        if (takingDamage == 0)
        {
            countChangeDamageSlider = 0;
            return;
        }

        if (Mathf.RoundToInt(healthValue) > Mathf.RoundToInt(damageValue))
        {
            ChangeCurrentDamageValue(true);
        }
        else if(Mathf.RoundToInt(healthValue) < Mathf.RoundToInt(damageValue))
        {
            ChangeCurrentDamageValue(false);
        }
        else
        {
            takingDamage = 0;
        }
    }
    void ChangeCurrentDamageValue(bool increase)
    {
        countChangeDamageSlider += Time.deltaTime;
        if(countChangeDamageSlider >= GameConfig.damageSliderChangeWaitTime)
        {
            if (increase)
            {
                playerInfo.playerCurrentHUD.currentDamageSlider.value = playerInfo.PlayerData.health;
            }
            else
            {
                playerInfo.playerCurrentHUD.currentDamageSlider.value -= takingDamage * 2 * Time.deltaTime;
            }
        }
    }
    void FungusDie()
    {
        EventManager.ActionOnFungusDie();
    }
}
