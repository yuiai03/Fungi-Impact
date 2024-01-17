using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusHealth : MonoBehaviour
{
    [SerializeField] private float countChangeDamageSlider;
    [SerializeField] private int takingDamage;

    private FungusInfoReader fungusInfo;
    private FungusCurrentStatusHUD fungusCurrentStatusHUD;
    private void Awake()
    {
        fungusInfo = GetComponent<FungusInfoReader>();

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
    void OnSwitchFungus(FungusInfoReader oldFungusInfo, FungusInfoReader newFungusInfo, FungusCurrentStatusHUD fungusCurrentStatusHUD)
    {
        this.fungusCurrentStatusHUD = fungusCurrentStatusHUD;
        var fungusData = newFungusInfo.FungusData; 

        fungusCurrentStatusHUD.SetCurrentHealthSliderInit(0, fungusData.maxHealth);
        fungusCurrentStatusHUD.SetCurrentDamageSliderInit(0, fungusData.maxHealth);
        fungusCurrentStatusHUD.SetCurrentHealthSlider(fungusData.health);
        fungusCurrentStatusHUD.SetCurrentDamageSlider(fungusData.health);
        fungusCurrentStatusHUD.SetHealthText(fungusData.health, fungusData.maxHealth);
        fungusCurrentStatusHUD.SetLvText(fungusData.lv);
    }
    public void TakeDamage(int value)
    {
        fungusInfo.FungusData.health -= value;
        fungusCurrentStatusHUD.SetCurrentHealthSlider(fungusInfo.FungusData.health);
        fungusCurrentStatusHUD.SetHealthText(fungusInfo.FungusData.health, fungusInfo.FungusData.maxHealth);
        takingDamage = value;

        if (fungusInfo.FungusData.health <= 0)
        {
            fungusInfo.FungusData.health = 0;
            FungusDie();         
        }
    }
    void UpdateCurrentDamageSlider()
    {
        if (fungusCurrentStatusHUD == null) return;
        float healthValue = fungusCurrentStatusHUD.currentHealthSlider.value;
        float damageValue = fungusCurrentStatusHUD.currentDamageSlider.value;


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
                fungusCurrentStatusHUD.currentDamageSlider.value = fungusInfo.FungusData.health;
            }
            else
            {
                fungusCurrentStatusHUD.currentDamageSlider.value -= takingDamage * 2 * Time.deltaTime;
            }
        }
    }
    void FungusDie()
    {
        EventManager.ActionOnFungusDie();
    }
}
