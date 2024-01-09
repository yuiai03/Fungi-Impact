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
}
