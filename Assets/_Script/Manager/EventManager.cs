using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void OnPickFungus(int slotIndex, FungusPackedConfig config);
    public static event OnPickFungus onPickFungus;

    public delegate void OnUnPickFungus(int slotIndex, FungusPackedConfig config);
    public static event OnUnPickFungus onUnPickFungus;

    public delegate void OnSwitchFungus(FungusData fungusData);
    public static event OnSwitchFungus onSwitchFungus;

    public delegate void OnShowInfoBoss(BossSlot bossSlot, bool state);
    public static event OnShowInfoBoss onShowInfoBoss;

    public delegate void OnSelectBoss(BossSlot bossSlot);
    public static event OnSelectBoss onSelectBoss;

    public static void ActionOnPickFungus(int slotIndex, FungusPackedConfig config)
    {
        onPickFungus?.Invoke(slotIndex, config);
    }
    public static void ActionOnUnPickFungus(int slotIndex, FungusPackedConfig config)
    {
        onUnPickFungus?.Invoke(slotIndex, config);
    }
    public static void ActionOnSwitchFungus(FungusData fungusData)
    {
        onSwitchFungus?.Invoke(fungusData);
    }
    public static void ActionOnShowInfoBoss(BossSlot bossSlot, bool state)
    {
        onShowInfoBoss?.Invoke(bossSlot, state);
    }
    public static void ActionOnSelectBoss(BossSlot bossSlot)
    {
        onSelectBoss?.Invoke(bossSlot);
    }
}
