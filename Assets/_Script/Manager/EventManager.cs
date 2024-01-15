using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void OnPickFungus(int slotIndex, FungusPackedConfig config);
    public static event OnPickFungus onPickFungus;

    public delegate void OnUnPickFungus(int slotIndex, FungusPackedConfig config);
    public static event OnUnPickFungus onUnPickFungus;

    public delegate void OnSwitchFungus(FungusInfoReader oldFungusInfo, FungusInfoReader newFungusInfo, FungusCurrentStatusHUD fungusCurrentStatusHUD);
    public static event OnSwitchFungus onSwitchFungus;

    public delegate void OnShowInfoBoss(BossSlot bossSlot, bool state);
    public static event OnShowInfoBoss onShowInfoBoss;

    public delegate void OnSelectBoss(BossSlot bossSlot);
    public static event OnSelectBoss onSelectBoss;

    public delegate void OnFungusDie();
    public static event OnFungusDie onFungusDie;

    public delegate void OnCameraChangeTarget(Transform targer);
    public static event OnCameraChangeTarget onCameraChangeTarget;

    public static void ActionOnPickFungus(int slotIndex, FungusPackedConfig config)
    {
        onPickFungus?.Invoke(slotIndex, config);
    }
    public static void ActionOnUnPickFungus(int slotIndex, FungusPackedConfig config)
    {
        onUnPickFungus?.Invoke(slotIndex, config);
    }
    public static void ActionOnSwitchFungus(FungusInfoReader oldFungusInfo, FungusInfoReader newFungusInfo, FungusCurrentStatusHUD fungusCurrentStatusHUD)
    {
        onSwitchFungus?.Invoke(oldFungusInfo, newFungusInfo, fungusCurrentStatusHUD);
    }
    public static void ActionOnShowInfoBoss(BossSlot bossSlot, bool state)
    {
        onShowInfoBoss?.Invoke(bossSlot, state);
    }
    public static void ActionOnSelectBoss(BossSlot bossSlot)
    {
        onSelectBoss?.Invoke(bossSlot);
    }   
    public static void ActionOnFungusDie()
    {
        onFungusDie?.Invoke();
    }
    public static void ActionOnCameraChangeTarget(Transform target)
    {
        onCameraChangeTarget?.Invoke(target);
    }
}
