using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLv : MonoBehaviour
{
    private PlayerInfoReader playerInfo;
    private void Awake()
    {
        playerInfo = GetComponent<PlayerInfoReader>();

        EventManager.onSwitchFungus += OnSwitchFungus;
    }
    private void OnDestroy()
    {
        EventManager.onSwitchFungus -= OnSwitchFungus;
    }
    private void Update()
    {

    }
    void OnSwitchFungus(FungusData fungusData)
    {
        playerInfo.playerCurrentHUD.SetLvText(fungusData.lv);
    }
}
