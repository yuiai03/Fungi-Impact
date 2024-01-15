using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusLv : MonoBehaviour
{
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

    }
    void OnSwitchFungus(FungusInfoReader oldDungusInfo, FungusInfoReader newFungusInfo, FungusCurrentStatusHUD fungusCurrentStatusHUD)
    {
        this.fungusCurrentStatusHUD = fungusCurrentStatusHUD;

        this.fungusCurrentStatusHUD.SetLvText(newFungusInfo.FungusData.lv);
    }
}
