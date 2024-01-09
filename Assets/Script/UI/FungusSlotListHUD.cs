using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusSlotListHUD : MonoBehaviour
{

    [SerializeField] private FungusSlotHUD fungusSlotHUDPrefab;
    [SerializeField] private List<FungusSlotHUD> fungusSlotHUDList = new List<FungusSlotHUD>();
    public List<FungusSlotHUD> FungusSlotHUDList { get => fungusSlotHUDList; }
    public void SetInit(List<FungusData> fungusDataList, List<KeyCode> inputSlot)
    {
        for (int i = 0; i < fungusDataList.Count; i++)
        {
            FungusData fungusData = fungusDataList[i];
            FungusSlotHUD fungusSlotHUD = Instantiate(fungusSlotHUDPrefab, transform);

            fungusSlotHUD.SetHealthSliderInit(0, fungusData.fungusStats.fungusMaxHealth);
            fungusSlotHUD.SetDamageSliderInit(0, fungusData.fungusStats.fungusMaxHealth);
            fungusSlotHUD.SetHealthSlider(fungusData.fungusStats.fungusMaxHealth);
            fungusSlotHUD.SetDamageSlider(fungusData.fungusStats.fungusMaxHealth);
            fungusSlotHUD.SetAvatar(fungusData.fungusConfig.fungusAvatar);
            fungusSlotHUD.SetName(fungusData.fungusConfig.fungusName);

            fungusSlotHUD.SetInputSlot(inputSlot[i]);
            fungusSlotHUD.SetChoseBgColor(fungusData.fungusConfig.fungusColor, GameConfig.unChosenSlotAlpha);

            fungusSlotHUDList.Add(fungusSlotHUD);
        }
    }
    public void SetSlotSelect(int index)
    {
        for (int i = 0; i < fungusSlotHUDList.Count; i++)
        {
            var fungusSlot = fungusSlotHUDList[i];
            Color color = fungusSlot.GetChoseBgColor();

            if (i == index) fungusSlot.SetChoseBgColor(color, GameConfig.choseSlotAlpha);
            else fungusSlot.SetChoseBgColor(color, GameConfig.unChosenSlotAlpha);
        }
    }
}
