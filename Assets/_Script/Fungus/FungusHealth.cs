using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusHealth : MonoBehaviour
{
    public Action<int> OnTakeDamageEvent;
    public Action OnDiedEvent;

    private FungusData fungusData;
    private void Awake()
    {
        EventManager.onSwitchFungus += OnSwitchFungus;
    }
    private void OnDestroy()
    {
        EventManager.onSwitchFungus -= OnSwitchFungus;
    }

    void OnSwitchFungus(FungusInfoReader fungusInfo, FungusCurrentStatusHUD fungusCurrentStatusHUD)
    {
        fungusData = fungusInfo.FungusData; 

    }
    public void TakeDamage(int value)
    {
        fungusData.health -= value;

        var takingDamage = value;

        OnTakeDamageEvent?.Invoke(takingDamage);

        if (fungusData.health <= 0)
        {
            fungusData.health = 0;
            OnDiedEvent?.Invoke();
        }
    }

}
