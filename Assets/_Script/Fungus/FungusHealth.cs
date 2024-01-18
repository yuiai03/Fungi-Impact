using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusHealth : MonoBehaviour
{
    [SerializeField] private int takingDamage;

    private FungusInfoReader fungusInfo;
    private FungusCurrentStatusHUD fungusCurrentStatusHUD;

    private Coroutine updateCurrentDamageSliderCoroutine;
    private Coroutine updateDamageSlotSliderCoroutine;
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

        fungusInfo.fungusSlotHUD.SetHealthSlider(fungusInfo.FungusData.health);

        takingDamage = value;

        if (updateCurrentDamageSliderCoroutine != null) StopCoroutine(updateCurrentDamageSliderCoroutine);
        StartCoroutine(UpdateCurrentDamageSliderCoroutine());

        if (updateDamageSlotSliderCoroutine != null) StopCoroutine(updateDamageSlotSliderCoroutine);
        StartCoroutine(UpdateDamageSlotSliderCoroutine());

        if (fungusInfo.FungusData.health <= 0)
        {
            fungusInfo.FungusData.health = 0;
            FungusDie();         
        }
    }
    IEnumerator UpdateCurrentDamageSliderCoroutine()
    {
        float healthValue = fungusCurrentStatusHUD.currentHealthSlider.value;
        float damageValue = fungusCurrentStatusHUD.currentDamageSlider.value;

        yield return new WaitForSeconds(GameConfig.damageSliderChangeWaitTime);

        while (Mathf.RoundToInt(healthValue) < Mathf.RoundToInt(damageValue))
        {
            fungusCurrentStatusHUD.currentDamageSlider.value -= takingDamage * 2 * Time.deltaTime;
            yield return null;
        }
        
    }

    IEnumerator UpdateDamageSlotSliderCoroutine()
    {
        float healthValue = fungusInfo.fungusSlotHUD.healthSlider.value;
        float damageValue = fungusInfo.fungusSlotHUD.damageSlider.value;

        yield return new WaitForSeconds(GameConfig.damageSliderChangeWaitTime);

        while (Mathf.RoundToInt(healthValue) < Mathf.RoundToInt(damageValue))
        {
            fungusInfo.fungusSlotHUD.damageSlider.value -= takingDamage * 2 * Time.deltaTime;
            yield return null;
        }
    }
    void FungusDie()
    {
        EventManager.ActionOnFungusDie();
    }
}
